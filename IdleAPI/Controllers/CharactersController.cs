using IdleAPI.Models;
using IdleCore.Helpers;
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

        public CharactersController(UserManager<IdentityUser> userManager, IAccountManager accountManager, ICharacterManager characterManager,
            ISummonHelper summonHelper, ILevelCalculator levelCalculator, IStatCalculator statCalculator)
        {
            _userManager = userManager;
            _accountManager = accountManager;
            _characterManager = characterManager;
            _summonHelper = summonHelper;
            _levelCalculator = levelCalculator;
            _statCalculator = statCalculator;
        }

        // Display a list of characters on your account.
        [HttpGet]
        public async Task<ActionResult<CharacterModel[]>> Get()
        {
            // Authorize attribute should prevent not having a user but return 401 if something breaks
            var userId = _userManager.GetUserId(User);
            if (userId is null)
            {
                return Unauthorized();
            }

            var account = await _accountManager.GetOrCreateAccount(userId, true);

            return account.Characters.Select(c => new CharacterModel(c, _levelCalculator, _statCalculator)).ToArray();
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
    }
}
