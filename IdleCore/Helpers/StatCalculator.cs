using IdleCore.Model;

namespace IdleCore.Helpers;

public interface IStatCalculator
{
    int CalculateConstitution(Character character, int level);
    int CalculateDexterity(Character character, int level);
    int CalculateIntelligence(Character character, int level);
    int CalculateStrength(Character character, int level);
    int CalculateVitality(Character character, int level);
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
