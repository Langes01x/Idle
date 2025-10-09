using IdleCore.Helpers;
using IdleCore.Model;

namespace IdleAPI.Models;

public class DetailedCharacterModel : CharacterModel
{
    public DetailedCharacterModel(Character character, ILevelCalculator levelCalculator, IStatCalculator statCalculator)
        : base(character, levelCalculator, statCalculator)
    {
        PhysicalDamage = statCalculator.CalculatePhysicalDamage(character, Strength);
        AetherDamage = statCalculator.CalculateAetherDamage(character, Intelligence);

        CritRating = statCalculator.CalculateCritRating(character, Dexterity);
        CritMultiplier = statCalculator.CalculateCritMultiplier(character);
        ActionSpeed = statCalculator.CalculateActionSpeed(character);

        Health = statCalculator.CalculateHealth(character, Vitality);
        Armour = statCalculator.CalculateArmour(character, Constitution);
        Barrier = statCalculator.CalculateBarrier(character, Wisdom);
        Evasion = statCalculator.CalculateEvasion(character);

        FireResistance = statCalculator.CalculateFireResistance(character);
        ColdResistance = statCalculator.CalculateColdResistance(character);
        PoisonResistance = statCalculator.CalculatePoisonResistance(character);
    }

    public decimal PhysicalDamage { get; set; }
    public decimal AetherDamage { get; set; }

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
