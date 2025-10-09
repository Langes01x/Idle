using IdleCore.Model;

namespace IdleCore.Helpers;

public interface IStatCalculator
{
    /// <summary>
    /// Calculate the constitution of the character.
    /// </summary>
    /// <param name="character">Character to calculate for.</param>
    /// <param name="level">Level of the character.</param>
    /// <returns>The character's constitution.</returns>
    int CalculateConstitution(Character character, int level);

    /// <summary>
    /// Calculate the dexterity of the character.
    /// </summary>
    /// <param name="character">Character to calculate for.</param>
    /// <param name="level">Level of the character.</param>
    /// <returns>The character's dexterity.</returns>
    int CalculateDexterity(Character character, int level);

    /// <summary>
    /// Calculate the intelligence of the character.
    /// </summary>
    /// <param name="character">Character to calculate for.</param>
    /// <param name="level">Level of the character.</param>
    /// <returns>The character's intelligence.</returns>
    int CalculateIntelligence(Character character, int level);

    /// <summary>
    /// Calculate the strength of the character.
    /// </summary>
    /// <param name="character">Character to calculate for.</param>
    /// <param name="level">Level of the character.</param>
    /// <returns>The character's strength.</returns>
    int CalculateStrength(Character character, int level);

    /// <summary>
    /// Calculate the Vitality of the character.
    /// </summary>
    /// <param name="character">Character to calculate for.</param>
    /// <param name="level">Level of the character.</param>
    /// <returns>The character's vitality.</returns>
    int CalculateVitality(Character character, int level);

    /// <summary>
    /// Calculate the wisdom of the character.
    /// </summary>
    /// <param name="character">Character to calculate for.</param>
    /// <param name="level">Level of the character.</param>
    /// <returns>The character's wisdom.</returns>
    int CalculateWisdom(Character character, int level);

    /// <summary>
    /// Calculate the physical damage of the character.
    /// </summary>
    /// <param name="character">Character to calculate for.</param>
    /// <param name="strength">Strength of the character.</param>
    /// <returns>The character's physical damage.</returns>
    decimal CalculatePhysicalDamage(Character character, int strength);

    /// <summary>
    /// Calculate the aether damage of the character.
    /// </summary>
    /// <param name="character">Character to calculate for.</param>
    /// <param name="intelligence">Intelligence of the character.</param>
    /// <returns>The character's aether damage.</returns>
    decimal CalculateAetherDamage(Character character, int intelligence);

    /// <summary>
    /// Calculate the crit rating of the character.
    /// </summary>
    /// <param name="character">Character to calculate for.</param>
    /// <param name="dexterity">Dexterity of the character.</param>
    /// <returns>The character's crit rating.</returns>
    int CalculateCritRating(Character character, int dexterity);

    /// <summary>
    /// Calculate the crit multiplier of the character.
    /// </summary>
    /// <param name="character">Character to calculate for.</param>
    /// <returns>The character's crit multiplier.</returns>
    decimal CalculateCritMultiplier(Character character);

    /// <summary>
    /// Calculate the action speed of the character.
    /// </summary>
    /// <param name="character">Character to calculate for.</param>
    /// <returns>The character's action speed.</returns>
    decimal CalculateActionSpeed(Character character);

    /// <summary>
    /// Calculate the health of the character.
    /// </summary>
    /// <param name="character">Character to calculate for.</param>
    /// <param name="vitality">Vitality of the character.</param>
    /// <returns>The character's health.</returns>
    int CalculateHealth(Character character, int vitality);

    /// <summary>
    /// Calculate the armour of the character.
    /// </summary>
    /// <param name="character">Character to calculate for.</param>
    /// <param name="constitution">Constitution of the character.</param>
    /// <returns>The character's armour.</returns>
    int CalculateArmour(Character character, int constitution);

    /// <summary>
    /// Calculate the barrier of the character.
    /// </summary>
    /// <param name="character">Character to calculate for.</param>
    /// <param name="wisdom">Wisdom of the character.</param>
    /// <returns>The character's barrier.</returns>
    int CalculateBarrier(Character character, int wisdom);

    /// <summary>
    /// Calculate the evasion of the character.
    /// </summary>
    /// <param name="character">Character to calculate for.</param>
    /// <returns>The character's evasion.</returns>
    decimal CalculateEvasion(Character character);

    /// <summary>
    /// Calculate the fire resistance of the character.
    /// </summary>
    /// <param name="character">Character to calculate for.</param>
    /// <returns>The character's fire resistance.</returns>
    decimal CalculateFireResistance(Character character);

    /// <summary>
    /// Calculate the cold resistance of the character.
    /// </summary>
    /// <param name="character">Character to calculate for.</param>
    /// <returns>The character's cold resistance.</returns>
    decimal CalculateColdResistance(Character character);

    /// <summary>
    /// Calculate the poison resistance of the character.
    /// </summary>
    /// <param name="character">Character to calculate for.</param>
    /// <returns>The character's poison resistance.</returns>
    decimal CalculatePoisonResistance(Character character);
}

public class StatCalculator : IStatCalculator
{
    public int CalculateStrength(Character character, int level)
    {
        return (character.Class.HasFlag(StatEnum.Strength) ? 2 : 0) * level * (int)character.Rarity;
    }

    public int CalculateIntelligence(Character character, int level)
    {
        return (character.Class.HasFlag(StatEnum.Intelligence) ? 2 : 0) * level * (int)character.Rarity;
    }

    public int CalculateDexterity(Character character, int level)
    {
        return (character.Class.HasFlag(StatEnum.Dexterity) ? 2 : 1) * level * (int)character.Rarity;
    }

    public int CalculateVitality(Character character, int level)
    {
        return (character.Class.HasFlag(StatEnum.Vitality) ? 2 : 1) * level * (int)character.Rarity;
    }

    public int CalculateConstitution(Character character, int level)
    {
        return (character.Class.HasFlag(StatEnum.Constitution) ? 2 : 1) * level * (int)character.Rarity;
    }

    public int CalculateWisdom(Character character, int level)
    {
        return (character.Class.HasFlag(StatEnum.Wisdom) ? 2 : 1) * level * (int)character.Rarity;
    }

    public decimal CalculatePhysicalDamage(Character character, int strength)
    {
        return strength == 0 ? 0 : 10 + (strength / 4.0m);
    }

    public decimal CalculateAetherDamage(Character character, int intelligence)
    {
        return intelligence == 0 ? 0 : 10 + (intelligence / 4.0m);
    }

    public int CalculateCritRating(Character character, int dexterity)
    {
        return dexterity;
    }

    public decimal CalculateCritMultiplier(Character character)
    {
        return 2.0m;
    }

    public decimal CalculateActionSpeed(Character character)
    {
        return 1.0m;
    }

    public int CalculateHealth(Character character, int vitality)
    {
        return 100 + (vitality * 10);
    }

    public int CalculateArmour(Character character, int constitution)
    {
        return constitution;
    }

    public int CalculateBarrier(Character character, int wisdom)
    {
        return wisdom;
    }

    public decimal CalculateEvasion(Character character)
    {
        return 0.05m;
    }

    public decimal CalculateFireResistance(Character character)
    {
        return 0m;
    }

    public decimal CalculateColdResistance(Character character)
    {
        return 0m;
    }

    public decimal CalculatePoisonResistance(Character character)
    {
        return 0m;
    }
}
