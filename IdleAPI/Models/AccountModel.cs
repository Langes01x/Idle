using IdleCore.Model;

namespace IdleAPI.Models;

public class AccountModel
{
    public AccountModel(Account account, string email)
    {
        Id = account.Id;
        Email = email;
        LastIdleCollection = account.LastIdleCollection;
        LevelsCleared = account.LevelsCleared;

        Experience = account.Experience;
        Gold = account.Gold;
        Diamonds = account.Diamonds;

        ExperienceAccumulationRate = account.GetExperienceAccumulationRate();
        GoldAccumulationRate = account.GetGoldAccumulationRate();
        DiamondAccumulationRate = account.GetDiamondAccumulationRate();
    }

    public string Id { get; set; }
    public string Email { get; set; }
    public DateTime LastIdleCollection { get; set; }
    public long LevelsCleared { get; set; }

    public long Experience { get; set; }
    public long Gold { get; set; }
    public long Diamonds { get; set; }

    public long ExperienceAccumulationRate { get; set; }
    public long GoldAccumulationRate { get; set; }
    public long DiamondAccumulationRate { get; set; }
}
