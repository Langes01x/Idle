using IdleCore.Model;

namespace IdleCore.Helpers;

public interface ILevelCompletionHelper
{
    /// <summary>
    /// Update account when level is cleared.
    /// </summary>
    /// <param name="account">Account to update.</param>
    /// <param name="area">Area the level is in.</param>
    /// <param name="level">Level completed.</param>
    /// <param name="combatSummary">Combat summary.</param>
    void CompleteLevel(Account account, Area area, Level level, CombatSummary combatSummary);
}

public class LevelCompletionHelper : ILevelCompletionHelper
{
    public void CompleteLevel(Account account, Area area, Level level, CombatSummary combatSummary)
    {
        var areaLevelNumber = (area.Number - 1) * 20 + level.Number;
        if (account.LevelsCleared + 1 == areaLevelNumber)
        {
            account.Diamonds += level.DiamondReward;
            account.Experience += level.ExperienceReward;
            account.Gold += level.GoldReward;
            account.LevelsCleared = areaLevelNumber;
            combatSummary.RewardsGiven = true;
        }
    }
}
