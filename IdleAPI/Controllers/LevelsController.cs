using IdleAPI.Models;
using IdleCore.Helpers;
using IdleCore.Helpers.Combat;
using IdleCore.Model;
using IdleDB.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace IdleAPI.Controllers;

[Authorize]
[ApiController]
[Route("Areas/{areaId:int}/[controller]")]
public class LevelsController : ControllerBase
{
    private readonly UserManager<IdentityUser> _userManager;
    private readonly IAreaManager _areaManager;
    private readonly IPartyManager _partyManager;
    private readonly ICombatHelper _combatHelper;
    private readonly ICombatSummaryManager _combatSummaryManager;
    private readonly IAccountManager _accountManager;
    private readonly ILevelCompletionHelper _levelCompletionHelper;

    public LevelsController(IAreaManager areaManager,
        UserManager<IdentityUser> userManager,
        IPartyManager partyManager,
        ICombatHelper combatHelper,
        ICombatSummaryManager combatSummaryManager,
        IAccountManager accountManager,
        ILevelCompletionHelper levelCompletionHelper)
    {
        _areaManager = areaManager;
        _userManager = userManager;
        _partyManager = partyManager;
        _combatHelper = combatHelper;
        _combatSummaryManager = combatSummaryManager;
        _accountManager = accountManager;
        _levelCompletionHelper = levelCompletionHelper;
    }

    // Display a level.
    [HttpGet("{levelId:int}")]
    public async Task<ActionResult<LevelModel>> GetLevel(int areaId, int levelId)
    {
        var level = await _areaManager.GetLevelDetails(areaId, levelId);
        if (level == null)
        {
            return NotFound();
        }

        return new LevelModel(level)
        {
            BackEnemy = level.BackEnemy == null ? null : new EnemyModel(level.BackEnemy),
            MiddleEnemy = level.MiddleEnemy == null ? null : new EnemyModel(level.MiddleEnemy),
            FrontEnemy = level.FrontEnemy == null ? null : new EnemyModel(level.FrontEnemy),
        };
    }

    private CombatSummaryModel CreateCombatSummaryModel(CombatSummary combatSummary)
    {
        var combatSummaryModel = new CombatSummaryModel(combatSummary)
        {
            Level = new LevelModel(combatSummary.Level)
            {
                BackEnemy = combatSummary.Level.BackEnemy == null ? null : new EnemyModel(combatSummary.Level.BackEnemy),
                MiddleEnemy = combatSummary.Level.MiddleEnemy == null ? null : new EnemyModel(combatSummary.Level.MiddleEnemy),
                FrontEnemy = combatSummary.Level.FrontEnemy == null ? null : new EnemyModel(combatSummary.Level.FrontEnemy),
            },
            CombatActions = combatSummary.CombatActions.Select(a => new CombatActionModel(a)).ToArray(),
        };
        if (combatSummary.BackCharacter != null)
        {
            combatSummaryModel.BackCharacter = new CharacterSnapshotModel(combatSummary.BackCharacter);
        }
        if (combatSummary.MiddleCharacter != null)
        {
            combatSummaryModel.MiddleCharacter = new CharacterSnapshotModel(combatSummary.MiddleCharacter);
        }
        if (combatSummary.FrontCharacter != null)
        {
            combatSummaryModel.FrontCharacter = new CharacterSnapshotModel(combatSummary.FrontCharacter);
        }
        return combatSummaryModel;
    }

    // Attempt a level.
    [HttpPost("{levelId:int}/Attempt")]
    public async Task<ActionResult<CombatSummaryModel>> AttemptLevel(int areaId, int levelId, int partyId)
    {
        // Authorize attribute should prevent not having a user but return 401 if something breaks
        var userId = _userManager.GetUserId(User);
        if (userId is null)
        {
            return Unauthorized();
        }

        var account = await _accountManager.GetOrCreateAccount(userId);

        var area = await _areaManager.GetArea(areaId);
        if (area == null)
        {
            return NotFound();
        }

        var level = await _areaManager.GetLevelDetails(areaId, levelId);
        if (level == null)
        {
            return NotFound();
        }

        var party = await _partyManager.GetParty(partyId, userId, true);
        if (party == null)
        {
            return NotFound();
        }

        if (party.BackCharacter == null &&
            party.MiddleCharacter == null &&
            party.FrontCharacter == null)
        {
            return BadRequest("Party has no characters.");
        }

        if (level.BackEnemy == null &&
            level.MiddleEnemy == null &&
            level.FrontEnemy == null)
        {
            return BadRequest("Level has no enemies.");
        }

        if (account.LevelsCleared + 1 < (area.Number - 1) * 20 + level.Number)
        {
            return BadRequest("Previous levels must be cleared first.");
        }

        var combatSummary = _combatHelper.AttemptLevel(userId, area, level, party);
        _combatSummaryManager.AddCombatSummary(combatSummary);
        if (combatSummary.Result == CombatResultEnum.Won)
        {
            _levelCompletionHelper.CompleteLevel(account, area, level);
        }
        await _combatSummaryManager.SaveChanges();

        return CreateCombatSummaryModel(combatSummary);
    }

    // Get past attempts for a level.
    [HttpGet("{levelId:int}/Attempts")]
    public async Task<ActionResult<CombatSummaryModel[]>> GetLevelAttempts(int areaId, int levelId)
    {
        // Authorize attribute should prevent not having a user but return 401 if something breaks
        var userId = _userManager.GetUserId(User);
        if (userId is null)
        {
            return Unauthorized();
        }

        var level = await _areaManager.GetLevelDetails(areaId, levelId);
        if (level == null)
        {
            return NotFound();
        }

        return (await _combatSummaryManager.GetCombatSummaries(levelId, userId)).Select(s => CreateCombatSummaryModel(s)).ToArray();
    }
}
