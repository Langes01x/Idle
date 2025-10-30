namespace IdleCore.Model;

public class Level
{
    public int Id { get; set; }

    public int AreaId { get; set; }

    public int Number { get; set; }

    public int ExperienceReward { get; set; }
    public int GoldReward { get; set; }
    public int DiamondReward { get; set; }

    public int? BackEnemyId { get; set; }
    public Enemy? BackEnemy { get; set; }

    public int? MiddleEnemyId { get; set; }
    public Enemy? MiddleEnemy { get; set; }

    public int? FrontEnemyId { get; set; }
    public Enemy? FrontEnemy { get; set; }
}
