using IdleCore.Model;

namespace IdleCore.Helpers;

public interface ISummonHelper
{
    int SummonCost { get; }

    IEnumerable<Character> SummonCharacters(Account account, int quantity);
}

public class SummonHelper : ISummonHelper
{
    private readonly Random _rng;

    private readonly (RarityEnum, double)[] _rarityTable = [
        (RarityEnum.Common, 0.5),
        (RarityEnum.Uncommon, 0.25),
        (RarityEnum.Rare, 0.15),
        (RarityEnum.Epic, 0.07),
        (RarityEnum.Legendary, 0.03),
    ];

    public SummonHelper(Random rng)
    {
        _rng = rng;
    }

    /// <summary>
    /// The cost to summon one character.
    /// </summary>
    public int SummonCost => 3600;

    /// <summary>
    /// Summon
    /// </summary>
    /// <param name="accountId"></param>
    /// <param name="quantity"></param>
    /// <returns></returns>
    public IEnumerable<Character> SummonCharacters(Account account, int quantity)
    {
        while (quantity > 0)
        {
            quantity--;
            yield return SummonCharacter(account);
        }
    }

    /// <summary>
    /// Generate a random character.
    /// </summary>
    /// <param name="accountId">Id for the account this character belongs to.</param>
    /// <returns>The newly generated character.</returns>
    private Character SummonCharacter(Account account)
    {
        account.Diamonds -= SummonCost;
        var newCharacter = new Character
        {
            AccountId = account.Id,
            Level = 1,
            Rarity = GenerateRandomRarity(),
            Class = GenerateRandomClass(),
            FirstName = GenerateRandomFirstName(),
            LastName = GenerateRandomLastName(),
        };
        account.Characters.Add(newCharacter);
        return newCharacter;
    }

    /// <summary>
    /// Generate a random rarity based on rarity percentages in the rarity table.
    /// </summary>
    /// <returns>A random weighted rarity.</returns>
    private RarityEnum GenerateRandomRarity()
    {
        var rarityRoll = _rng.NextDouble();
        var rollCap = 0.0;
        foreach (var rarityTier in _rarityTable)
        {
            rollCap += rarityTier.Item2;
            if (rollCap > rarityRoll)
            {
                return rarityTier.Item1;
            }
        }
        return RarityEnum.Common;
    }

    /// <summary>
    /// Generate a random first name.
    /// </summary>
    /// <returns>A random first name.</returns>
    private FirstNameEnum GenerateRandomFirstName()
    {
        return (FirstNameEnum)_rng.Next((int)FirstNameEnum._Count);
    }

    /// <summary>
    /// Generate a random last name.
    /// </summary>
    /// <returns>A random last name.</returns>
    private LastNameEnum GenerateRandomLastName()
    {
        return (LastNameEnum)_rng.Next((int)LastNameEnum._Count);
    }

    /// <summary>
    /// Generate a random class.
    /// </summary>
    /// <returns>A random class.</returns>
    private StatEnum GenerateRandomClass()
    {
        // All classes are comprised of a main damage stat and two secondary stats
        var mainStat = _rng.Next(2) == 0 ? StatEnum.Strength : StatEnum.Intelligence;
        var secondaryStats = new List<StatEnum> { StatEnum.Dexterity, StatEnum.Vitality, StatEnum.Constitution, StatEnum.Wisdom };
        var secondStat = secondaryStats[_rng.Next(secondaryStats.Count)];
        secondaryStats.Remove(secondStat);
        var thirdStat = secondaryStats[_rng.Next(secondaryStats.Count)];
        return mainStat | secondStat | thirdStat;
    }
}
