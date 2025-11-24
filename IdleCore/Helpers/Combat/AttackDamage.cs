namespace IdleCore.Helpers.Combat;

public class AttackDamage
{
    public decimal PhysicalDamageDealt { get; set; }
    public decimal AetherDamageDealt { get; set; }
    public decimal FireDamageDealt { get; set; }
    public decimal ColdDamageDealt { get; set; }
    public decimal PoisonDamageDealt { get; set; }
    public bool IsCrit { get; set; }
    public bool IsMiss { get; set; }
}
