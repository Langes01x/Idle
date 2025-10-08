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
}
