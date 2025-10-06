using IdleCore.Model;

namespace IdleCore.Helpers;

public interface ICollectionHelper
{
    /// <summary>
    /// Collect idle rewards for an account.
    /// </summary>
    /// <param name="account">The account to collect idle rewards for.</param>
    void CollectIdleRewards(Account account);

    /// <summary>
    /// Get the number of seconds between the given UTC date/time and the last time collection happened for the account.
    /// </summary>
    /// <param name="currentUtcTime">The current UTC date/time.</param>
    /// <param name="account">The account to calculate for.</param>
    /// <returns>The number of seconds since last collection.</returns>
    long GetAccumulationTime(DateTime currentUtcTime, Account account);

    /// <summary>
    /// Get the rate that the account accumulates diamonds.
    /// </summary>
    /// <param name="account">The account to calculate for.</param>
    /// <returns>The diamond accumulation rate.</returns>
    long GetDiamondAccumulationRate(Account account);

    /// <summary>
    /// Get the rate that the account accumulates experience.
    /// </summary>
    /// <param name="account">The account to calculate for.</param>
    /// <returns>The experience accumulation rate.</returns>
    long GetExperienceAccumulationRate(Account account);

    /// <summary>
    /// Get the rate that the account accumulates gold.
    /// </summary>
    /// <param name="account">The account to calculate for.</param>
    /// <returns>The gold accumulation rate.</returns>
    long GetGoldAccumulationRate(Account account);
}

public class CollectionHelper : ICollectionHelper
{
    public void CollectIdleRewards(Account account)
    {
        var now = DateTime.UtcNow;
        var secondsOfAccumulation = GetAccumulationTime(now, account);
        account.LastIdleCollection = now;
        account.Experience += secondsOfAccumulation * GetExperienceAccumulationRate(account);
        account.Gold += secondsOfAccumulation * GetGoldAccumulationRate(account);
        account.Diamonds += secondsOfAccumulation * GetDiamondAccumulationRate(account);
    }

    public long GetAccumulationTime(DateTime currentUtcTime, Account account)
    {
        return (long)(currentUtcTime - account.LastIdleCollection).TotalSeconds;
    }

    public long GetDiamondAccumulationRate(Account account)
    {
        return 1;
    }

    public long GetExperienceAccumulationRate(Account account)
    {
        return account.LevelsCleared + 1;
    }

    public long GetGoldAccumulationRate(Account account)
    {
        return account.LevelsCleared + 1;
    }
}
