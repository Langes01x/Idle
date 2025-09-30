using IdleCore.Model;

namespace IdleAPI.Models;

public class CharacterModel
{
    public CharacterModel(Character character)
    {
        Id = character.Id;
        Level = character.Level;
        Rarity = character.Rarity.ToString();
        Class = character.Class.ToString();
        FirstName = character.FirstName.ToString();
        LastName = character.LastName.ToString();
        Strength = character.Strength;
        Intelligence = character.Intelligence;
        Dexterity = character.Dexterity;
        Vitality = character.Vitality;
        Wisdom = character.Wisdom;
    }

    public int Id { get; set; }

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
