using IdleCore.Model;

namespace IdleCore.Helpers;

public interface ISummonHelper
{
    /// <summary>
    /// The cost to summon one character.
    /// </summary>
    int SummonCost { get; }

    /// <summary>
    /// Summon a number of characters.
    /// </summary>
    /// <param name="account">Account to summon characters for.</param>
    /// <param name="quantity">Quantity of characters to summon.</param>
    /// <param name="pullIsFree">A flag determining whether the pull is free.</param>
    /// <returns>The characters which were summoned.</returns>
    IEnumerable<Character> SummonCharacters(Account account, int quantity, bool pullIsFree);
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

    private readonly (RarityEnum, double)[] _guaranteedRarityTable = [
        (RarityEnum.Rare, 0.90),
        (RarityEnum.Epic, 0.07),
        (RarityEnum.Legendary, 0.03),
    ];

    public SummonHelper(Random rng)
    {
        _rng = rng;
    }

    public int SummonCost => 3600;

    public IEnumerable<Character> SummonCharacters(Account account, int quantity, bool pullIsFree)
    {
        for (var pullCount = 1; pullCount <= quantity; pullCount++)
        {
            yield return SummonCharacter(account, pullIsFree, pullCount % 10 == 0);
        }
    }

    /// <summary>
    /// Generate a random character.
    /// </summary>
    /// <param name="accountId">Id for the account this character belongs to.</param>
    /// <param name="pullIsFree">A flag determining whether the pull is free.</param>
    /// <param name="guaranteedPull">A flag determining whether the pull has a guaranteed rarity.</param>
    /// <returns>The newly generated character.</returns>
    private Character SummonCharacter(Account account, bool pullIsFree, bool guaranteedPull)
    {
        if (pullIsFree)
        {
            account.HasUsedFreePull = true;
        }
        else
        {
            account.Diamonds -= SummonCost;
        }
        var newCharacter = new Character
        {
            AccountId = account.Id,
            Experience = 0,
            Rarity = GenerateRandomRarity(guaranteedPull),
            Class = GenerateRandomClass(),
            FirstName = GenerateRandomFirstName(),
            LastName = GenerateRandomLastName(),
        };
        account.Characters.Add(newCharacter);
        return newCharacter;
    }

    /// <summary>
    /// Generate a random rarity based on rarity percentages in the rarity table.
    /// <param name="guaranteedPull">A flag determining whether the pull has a guaranteed rarity.</param>
    /// </summary>
    /// <returns>A random weighted rarity.</returns>
    private RarityEnum GenerateRandomRarity(bool guaranteedPull)
    {
        var rarityRoll = _rng.NextDouble();
        var rollCap = 0.0;
        var table = guaranteedPull ? _guaranteedRarityTable : _rarityTable;
        foreach (var rarityTier in table)
        {
            rollCap += rarityTier.Item2;
            if (rollCap > rarityRoll)
            {
                return rarityTier.Item1;
            }
        }
        // Should never be hit as the rarity table should total to 1.0 and
        // the random function always returns a value less than 1.0
        throw new Exception($"Could not generate rarity for roll {rarityRoll}. Ensure that the rarity table totals to 1.0");
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
