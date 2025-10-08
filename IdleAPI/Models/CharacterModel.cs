using IdleCore.Helpers;
using IdleCore.Model;

namespace IdleAPI.Models;

public class CharacterModel
{
    public CharacterModel(Character character, ILevelCalculator levelCalculator, IStatCalculator statCalculator)
    {
        Id = character.Id;

        Experience = character.Experience;
        ExperienceToNextLevel = levelCalculator.GetExperienceToNextLevel(character);
        Level = levelCalculator.GetLevel(character);

        Rarity = character.Rarity.ToString();
        Class = character.Class.ToString();
        FirstName = character.FirstName.ToString();
        LastName = character.LastName.ToString();

        Strength = statCalculator.CalculateStrength(character, Level);
        Intelligence = statCalculator.CalculateIntelligence(character, Level);
        Dexterity = statCalculator.CalculateDexterity(character, Level);
        Vitality = statCalculator.CalculateVitality(character, Level);
        Constitution = statCalculator.CalculateConstitution(character, Level);
        Wisdom = statCalculator.CalculateWisdom(character, Level);
    }

    public int Id { get; set; }

    public long Experience { get; set; }
    public long ExperienceToNextLevel { get; set; }
    public int Level { get; set; }

    public string Rarity { get; set; }
    public string Class { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }

    public int Strength { get; set; }
    public int Intelligence { get; set; }
    public int Dexterity { get; set; }
    public int Vitality { get; set; }
    public int Constitution { get; set; }
    public int Wisdom { get; set; }
}
