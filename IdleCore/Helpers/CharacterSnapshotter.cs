using IdleCore.Model;

namespace IdleCore.Helpers;

public interface ICharacterSnapshotter
{
    /// <summary>
    /// Create a snapshot of a character.
    /// </summary>
    /// <param name="character">Character to snapshot.</param>
    /// <returns>The snapshotted character.</returns>
    CharacterSnapshot TakeCharacterSnapshot(Character character);
}

public class CharacterSnapshotter : ICharacterSnapshotter
{
    private ILevelCalculator _levelCalculator;
    private IStatCalculator _statCalculator;

    public CharacterSnapshotter(ILevelCalculator levelCalculator, IStatCalculator statCalculator)
    {
        _levelCalculator = levelCalculator;
        _statCalculator = statCalculator;
    }

    public CharacterSnapshot TakeCharacterSnapshot(Character character)
    {
        var level = _levelCalculator.GetLevel(character);

        return new CharacterSnapshot
        {
            Level = level,
            Health = _statCalculator.CalculateHealth(character, _statCalculator.CalculateVitality(character, level)),
            Rarity = character.Rarity,
            Class = character.Class,
            FirstName = character.FirstName,
            LastName = character.LastName,
        };
    }
}
