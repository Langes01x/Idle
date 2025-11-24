using IdleCore.Model;

namespace IdleCore.Helpers.Combat;

public class CombatParticipant
{
    public PositionEnum Position { get; set; }
    public SideEnum Side { get; set; }
    public bool IsDead { get; set; }

    public int Level { get; set; }

    public decimal PhysicalDamage { get; set; }
    public decimal AetherDamage { get; set; }
    public decimal FireDamage { get; set; }
    public decimal ColdDamage { get; set; }
    public decimal PoisonDamage { get; set; }

    public decimal CritChance { get; set; }
    public decimal CritMultiplier { get; set; }
    public decimal ActionSpeed { get; set; }

    public int StartingHealth { get; set; }
    public decimal CurrentHealth { get; set; }
    public decimal ArmourEffect { get; set; }
    public decimal BarrierEffect { get; set; }
    public decimal Evasion { get; set; }

    public decimal FireResistance { get; set; }
    public decimal ColdResistance { get; set; }
    public decimal PoisonResistance { get; set; }
}
