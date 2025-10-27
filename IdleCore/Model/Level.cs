namespace IdleCore.Model;

public class Level
{
    public int Id { get; set; }

    public int AreaId { get; set; }

    public int Number { get; set; }

    public int ExperienceReward { get; set; }
    public int GoldReward { get; set; }
    public int DiamondReward { get; set; }

    public ICollection<Enemy> Enemies { get; } = default!;
}
