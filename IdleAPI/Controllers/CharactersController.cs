using IdleAPI.Models;
using IdleCore.Helpers;
using IdleCore.Model;
using IdleDB.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace IdleAPI.Controllers
{
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class CharactersController : ControllerBase
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IAccountManager _accountManager;
        private readonly ICharacterManager _characterManager;
        private readonly ISummonHelper _summonHelper;
        private readonly ILevelCalculator _levelCalculator;
        private readonly IStatCalculator _statCalculator;
        private readonly ICharacterSorter _characterSorter;
        private readonly IEnumMapper _enumMapper;

        public CharactersController(UserManager<IdentityUser> userManager, IAccountManager accountManager, ICharacterManager characterManager,
            ISummonHelper summonHelper, ILevelCalculator levelCalculator, IStatCalculator statCalculator, ICharacterSorter characterSorter,
            IEnumMapper enumMapper)
        {
            _userManager = userManager;
            _accountManager = accountManager;
            _characterManager = characterManager;
            _summonHelper = summonHelper;
            _levelCalculator = levelCalculator;
            _statCalculator = statCalculator;
            _characterSorter = characterSorter;
            _enumMapper = enumMapper;
        }

        // Display a list of characters on your account.
        [HttpGet]
        public async Task<ActionResult<GetCharactersModel>> Get(CharacterSortOrderEnum? sortOrder)
        {
            // Authorize attribute should prevent not having a user but return 401 if something breaks
            var userId = _userManager.GetUserId(User);
            if (userId is null)
            {
                return Unauthorized();
            }

            var account = await _accountManager.GetOrCreateAccount(userId, true);
            if (sortOrder is not null)
            {
                account.CharacterSortOrder = sortOrder.Value;
                await _accountManager.SaveChanges();
            }

            return new GetCharactersModel
            {
                Characters = _characterSorter
                    .SortCharacters(account.Characters, account.CharacterSortOrder)
                    .Select(c => new CharacterModel(c, _levelCalculator, _statCalculator))
                    .ToArray(),
                SummonCost = _summonHelper.SummonCost,
                SortOrderOptions = _enumMapper.GetEnumMapping<CharacterSortOrderEnum>(),
                DefaultSortOrder = account.CharacterSortOrder,
            };
        }

        // Display a character on your account.
        [HttpGet("{id:int}")]
        public async Task<ActionResult<CharacterModel>> Get(int id)
        {
            // Authorize attribute should prevent not having a user but return 401 if something breaks
            var userId = _userManager.GetUserId(User);
            if (userId is null)
            {
                return Unauthorized();
            }

            var character = await _characterManager.GetCharacter(id, userId);
            if (character == null)
            {
                return NotFound();
            }

            return new CharacterModel(character, _levelCalculator, _statCalculator);
        }

        // Summon new characters
        [HttpPost("Summon")]
        public async Task<ActionResult<CharacterModel[]>> Summon(int quantity)
        {
            // Authorize attribute should prevent not having a user but return 401 if something breaks
            var userId = _userManager.GetUserId(User);
            if (userId is null)
            {
                return Unauthorized();
            }

            // Need to load characters at the same time so EF will fix up the navigation property
            var account = await _accountManager.GetOrCreateAccount(userId, true);
            if (account.Diamonds < (quantity * _summonHelper.SummonCost))
            {
                ModelState.AddModelError("quantity", $"Not enough diamonds to summon {quantity} times");
                return BadRequest(ModelState);
            }

            var newCharacters = _summonHelper.SummonCharacters(account, quantity).ToArray();
            await _accountManager.SaveChanges();

            return newCharacters.Select(c => new CharacterModel(c, _levelCalculator, _statCalculator)).ToArray();
        }

        // Level up character
        [HttpPost("{id:int}/LevelUp")]
        public async Task<ActionResult<CharacterModel>> LevelUp(int id)
        {
            // Authorize attribute should prevent not having a user but return 401 if something breaks
            var userId = _userManager.GetUserId(User);
            if (userId is null)
            {
                return Unauthorized();
            }

            var character = await _characterManager.GetCharacter(id, userId);
            if (character == null)
            {
                return NotFound();
            }

            var account = await _accountManager.GetOrCreateAccount(userId);
            var experienceCost = _levelCalculator.GetExperienceToNextLevel(character);
            if (account.Experience < experienceCost)
            {
                ModelState.AddModelError("experience", $"Not enough experience to level up character {id}");
                return BadRequest(ModelState);
            }

            character.Experience += experienceCost;
            account.Experience -= experienceCost;
            await _accountManager.SaveChanges();

            return new CharacterModel(character, _levelCalculator, _statCalculator);
        }

        // Dismiss character
        [HttpPost("{id:int}/Dismiss")]
        public async Task<IActionResult> Dismiss(int id)
        {
            // Authorize attribute should prevent not having a user but return 401 if something breaks
            var userId = _userManager.GetUserId(User);
            if (userId is null)
            {
                return Unauthorized();
            }

            var character = await _characterManager.GetCharacter(id, userId);
            if (character == null)
            {
                return NotFound();
            }

            var account = await _accountManager.GetOrCreateAccount(userId);
            account.Experience += character.Experience;
            account.Diamonds += 100;
            _characterManager.DeleteCharacter(character);
            await _characterManager.SaveChanges();

            return Ok();
        }
    }
}
