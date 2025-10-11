using IdleAPI.Models;
using IdleCore.Helpers;
using IdleCore.Model;
using IdleDB.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace IdleAPI.Controllers;

[Authorize]
[ApiController]
[Route("[controller]")]
public class PartiesController : ControllerBase
{
    private readonly UserManager<IdentityUser> _userManager;
    private readonly IPartyManager _partyManager;
    private readonly ICharacterManager _characterManager;
    private readonly ILevelCalculator _levelCalculator;
    private readonly IStatCalculator _statCalculator;

    public PartiesController(UserManager<IdentityUser> userManager, IPartyManager partyManager,
        ICharacterManager characterManager, ILevelCalculator levelCalculator, IStatCalculator statCalculator)
    {
        _userManager = userManager;
        _partyManager = partyManager;
        _characterManager = characterManager;
        _levelCalculator = levelCalculator;
        _statCalculator = statCalculator;
    }

    // Display a list of parties on your account.
    [HttpGet]
    public async Task<ActionResult<PartyModel[]>> Get()
    {
        // Authorize attribute should prevent not having a user but return 401 if something breaks
        var userId = _userManager.GetUserId(User);
        if (userId is null)
        {
            return Unauthorized();
        }

        return (await _partyManager.GetParties(userId)).Select(CreatePartyModel).ToArray();
    }

    private PartyModel CreatePartyModel(Party party)
    {
        var partyModel = new PartyModel(party);
        if (party.BackCharacter != null)
        {
            partyModel.BackCharacter = new CharacterModel(party.BackCharacter, _levelCalculator, _statCalculator);
        }
        if (party.MiddleCharacter != null)
        {
            partyModel.MiddleCharacter = new CharacterModel(party.MiddleCharacter, _levelCalculator, _statCalculator);
        }
        if (party.FrontCharacter != null)
        {
            partyModel.FrontCharacter = new CharacterModel(party.FrontCharacter, _levelCalculator, _statCalculator);
        }
        return partyModel;
    }

    // Create or update party
    [HttpPost]
    public async Task<ActionResult<PartyModel>> CreateOrUpdate(PartyModel partyModel)
    {
        // Authorize attribute should prevent not having a user but return 401 if something breaks
        var userId = _userManager.GetUserId(User);
        if (userId is null)
        {
            return Unauthorized();
        }

        Party? party = null;
        if (partyModel.Id > 0)
        {
            // Update existing party
            party = await _partyManager.GetParty(partyModel.Id, userId);
            if (party == null)
            {
                return NotFound();
            }

            if (party.Name != partyModel.Name)
            {
                party.Name = partyModel.Name;
            }
            if (!await TryUpdateCharacter(c => party.BackCharacter = c, () => party.BackCharacterId = null, party.BackCharacterId, partyModel.BackCharacter, userId))
            {
                return BadRequest("Back character is not valid");
            }
            if (!await TryUpdateCharacter(c => party.MiddleCharacter = c, () => party.MiddleCharacterId = null, party.MiddleCharacterId, partyModel.MiddleCharacter, userId))
            {
                return BadRequest("Middle character is not valid");
            }
            if (!await TryUpdateCharacter(c => party.FrontCharacter = c, () => party.FrontCharacterId = null, party.FrontCharacterId, partyModel.FrontCharacter, userId))
            {
                return BadRequest("Front character is not valid");
            }
        }
        else
        {
            // Create new party
            party = new Party { AccountId = userId, Name = partyModel.Name };
            if (!await TryUpdateCharacter(c => party.BackCharacter = c, () => party.BackCharacterId = null, party.BackCharacterId, partyModel.BackCharacter, userId))
            {
                return BadRequest("Back character is not valid");
            }
            if (!await TryUpdateCharacter(c => party.MiddleCharacter = c, () => party.MiddleCharacterId = null, party.MiddleCharacterId, partyModel.MiddleCharacter, userId))
            {
                return BadRequest("Middle character is not valid");
            }
            if (!await TryUpdateCharacter(c => party.FrontCharacter = c, () => party.FrontCharacterId = null, party.FrontCharacterId, partyModel.FrontCharacter, userId))
            {
                return BadRequest("Front character is not valid");
            }
            _partyManager.CreateParty(party);
        }
        if ((party.BackCharacter != null && (party.BackCharacter == party.MiddleCharacter || party.BackCharacter == party.FrontCharacter)) ||
            (party.MiddleCharacter != null && party.MiddleCharacter == party.FrontCharacter))
        {
            return BadRequest("All characters in a party must be different");
        }

        await _partyManager.SaveChanges();
        return CreatePartyModel(party);
    }

    private async Task<bool> TryUpdateCharacter(Action<Character?> setCharacter, Action clearCharacterId,
        int? existingCharacterId, CharacterModel? newCharacter, string userId)
    {
        if (existingCharacterId != newCharacter?.Id)
        {
            if (newCharacter == null)
            {
                clearCharacterId();
            }
            else
            {
                var character = await _characterManager.GetCharacter(newCharacter.Id, userId);
                if (character == null)
                {
                    return false;
                }
                else
                {
                    setCharacter(character);
                }
            }
        }
        else if (existingCharacterId != null)
        {
            await _characterManager.GetCharacter(existingCharacterId.Value, userId);
        }
        return true;
    }

    // Delete party
    [HttpPost("{id:int}/Delete")]
    public async Task<IActionResult> Delete(int id)
    {
        // Authorize attribute should prevent not having a user but return 401 if something breaks
        var userId = _userManager.GetUserId(User);
        if (userId is null)
        {
            return Unauthorized();
        }

        var party = await _partyManager.GetParty(id, userId);
        if (party == null)
        {
            return NotFound();
        }

        _partyManager.DeleteParty(party);
        await _partyManager.SaveChanges();

        return Ok();
    }
}
