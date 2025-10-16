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
    // Level 3: 80 xp
    // Level 4: 256 xp
    // Level 5: 625 xp
    // Level 10: 10,001 xp
    // Level 20: 160,008 xp
    // Level 50: 6,250,321 xp
    // Level 100: 100,104,030 xp
    // Level 200: 12,422,639,408 xp
    private long CalculateExperienceRequirement(int level)
    {
        return (long)(Math.Pow(level, 4)
            + Math.Pow(2, level / 6.0)
            - 2);
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
