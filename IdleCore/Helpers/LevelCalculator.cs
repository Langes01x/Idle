using IdleCore.Model;

namespace IdleCore.Helpers;

public interface ILevelCalculator
{
    /// <summary>
    /// Get the amount of experience required for the character to reach the next level.
    /// </summary>
    /// <param name="character">The character to do the calculation for.</param>
    /// <returns>The experience required to level the character.</returns>
    long GetExperienceToNextLevel(Character character);

    /// <summary>
    /// Get the level of the character based on their experience.
    /// </summary>
    /// <param name="character">The character to calculate the level of.</param>
    /// <returns>The character's calculated level.</returns>
    int GetLevel(Character character);
}

public class LevelCalculator : ILevelCalculator
{
    private readonly List<(int level, long experience)> _levelExperienceMap;

    // Level 1: 0 xp
    // Level 2: 15 xp
    // Level 3: 138 xp
    // Level 4: 657 xp
    // Level 5: 2,172 xp
    // Level 10: 82,647 xp
    // Level 20: 2,902,497 xp
    // Level 50: 300,365,247 xp
    // Level 100: 9,802,960,497 xp
    // Level 200: 316,823,840,997 xp
    private long CalculateExperienceRequirement(int level)
    {
        return (long)Math.Pow(level, 5)
            - (2 * (long)Math.Pow(level, 4))
            + (3 * (long)Math.Pow(level, 3))
            - (4 * (long)Math.Pow(level, 2))
            + (5 * level)
            - 3;
    }

    public LevelCalculator()
    {
        _levelExperienceMap = [];
        for (int level = 1; level <= 200; level++)
        {
            _levelExperienceMap.Add((level, CalculateExperienceRequirement(level)));
        }
    }

    public int GetLevel(Character character)
    {
        var (level, experience) = _levelExperienceMap.FirstOrDefault(m => m.experience > character.Experience);
        return level == default ? 200 : level - 1;
    }

    public long GetExperienceToNextLevel(Character character)
    {
        var (level, experience) = _levelExperienceMap.First(m => m.experience > character.Experience);
        return experience == default ? long.MaxValue : experience - character.Experience;
    }
}
