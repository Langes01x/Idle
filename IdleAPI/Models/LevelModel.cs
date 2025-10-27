using System.Text.Json.Serialization;
using IdleCore.Model;

namespace IdleAPI.Models;

public class LevelModel
{
    [JsonConstructor]
    private LevelModel()
    {
    }

    public LevelModel(Level level)
    {
        Id = level.Id;
        Number = level.Number;
        ExperienceReward = level.ExperienceReward;
        GoldReward = level.GoldReward;
        DiamondReward = level.DiamondReward;
    }

    public int Id { get; set; }

    public int Number { get; set; }

    public int ExperienceReward { get; set; }
    public int GoldReward { get; set; }
    public int DiamondReward { get; set; }

    public EnemyModel[]? Enemies { get; set; }
}
