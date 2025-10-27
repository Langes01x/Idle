namespace IdleCore.Model;

public class Enemy
{
    public int Id { get; set; }

    public int LevelId { get; set; }

    public required string Name { get; set; }

    public decimal PhysicalDamage { get; set; }
    public decimal AetherDamage { get; set; }
    public decimal FireDamage { get; set; }
    public decimal ColdDamage { get; set; }
    public decimal PoisonDamage { get; set; }

    public int CritRating { get; set; }
    public decimal CritMultiplier { get; set; }
    public decimal ActionSpeed { get; set; }

    public int Health { get; set; }
    public int Armour { get; set; }
    public int Barrier { get; set; }
    public decimal Evasion { get; set; }

    public decimal FireResistance { get; set; }
    public decimal ColdResistance { get; set; }
    public decimal PoisonResistance { get; set; }
}
