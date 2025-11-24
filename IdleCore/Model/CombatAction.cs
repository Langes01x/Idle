namespace IdleCore.Model;

public class CombatAction
{
    public int Id { get; set; }

    public int CombatSummaryId { get; set; }

    public TimeSpan Time { get; set; }

    public SideEnum AttackerSide { get; set; }
    public PositionEnum AttackerPosition { get; set; }
    public PositionEnum DefenderPosition { get; set; }
    public bool IsDefenderDead { get; set; }

    public decimal PhysicalDamageDealt { get; set; }
    public decimal AetherDamageDealt { get; set; }
    public decimal FireDamageDealt { get; set; }
    public decimal ColdDamageDealt { get; set; }
    public decimal PoisonDamageDealt { get; set; }
    public bool IsCrit { get; set; }
    public bool IsMiss { get; set; }
}
