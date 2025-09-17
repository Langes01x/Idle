namespace IdleCore;

public class Account
{
    public required string Id { get; set; }
    public required DateTime LastIdleCollection { get; set; }
    public long Experience { get; set; }
    public long Gold { get; set; }
    public long Diamonds { get; set; }
    public long LevelsCleared { get; set; }

    public long GetExperienceAccumulationRate()
    {
        return LevelsCleared + 1;
    }

    public long GetGoldAccumulationRate()
    {
        return LevelsCleared + 1;
    }

    public long GetDiamondAccumulationRate()
    {
        return 1;
    }

    public long GetAccumulationTime(DateTime currentUtcTime)
    {
        return (long)(currentUtcTime - LastIdleCollection).TotalSeconds;
    }

    public void CollectIdleRewards()
    {
        var now = DateTime.UtcNow;
        var secondsOfAccumulation = GetAccumulationTime(now);
        LastIdleCollection = now;
        Experience += secondsOfAccumulation * GetExperienceAccumulationRate();
        Gold += secondsOfAccumulation * GetGoldAccumulationRate();
        Diamonds += secondsOfAccumulation * GetDiamondAccumulationRate();
    }
}
