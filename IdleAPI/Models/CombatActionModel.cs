using System.Text.Json.Serialization;
using IdleCore.Model;

namespace IdleAPI.Models;

public class CombatActionModel
{
    [JsonConstructor]
    public CombatActionModel()
    {
        AttackerSide = "";
        AttackerPosition = "";
        DefenderPosition = "";
    }

    public CombatActionModel(CombatAction combatAction)
    {
        Time = combatAction.Time;

        AttackerSide = combatAction.AttackerSide.ToString();
        AttackerPosition = combatAction.AttackerPosition.ToString();
        DefenderPosition = combatAction.DefenderPosition.ToString();
        IsDefenderDead = combatAction.IsDefenderDead;

        PhysicalDamageDealt = combatAction.PhysicalDamageDealt;
        AetherDamageDealt = combatAction.AetherDamageDealt;
        FireDamageDealt = combatAction.FireDamageDealt;
        ColdDamageDealt = combatAction.ColdDamageDealt;
        PoisonDamageDealt = combatAction.PoisonDamageDealt;
        IsCrit = combatAction.IsCrit;
        IsMiss = combatAction.IsMiss;
    }

    public TimeSpan Time { get; set; }

    public string AttackerSide { get; set; }
    public string AttackerPosition { get; set; }
    public string DefenderPosition { get; set; }
    public bool IsDefenderDead { get; set; }

    public decimal PhysicalDamageDealt { get; set; }
    public decimal AetherDamageDealt { get; set; }
    public decimal FireDamageDealt { get; set; }
    public decimal ColdDamageDealt { get; set; }
    public decimal PoisonDamageDealt { get; set; }
    public bool IsCrit { get; set; }
    public bool IsMiss { get; set; }
}
