using System.Text.Json.Serialization;
using IdleCore.Model;

namespace IdleAPI.Models;

public class CharacterSnapshotModel
{
    [JsonConstructor]
    private CharacterSnapshotModel()
    {
        Rarity = "";
        Class = "";
        FirstName = "";
        LastName = "";
    }

    public CharacterSnapshotModel(CharacterSnapshot characterSnapshot)
    {
        Level = characterSnapshot.Level;
        Health = characterSnapshot.Health;
        CurrentHealth = characterSnapshot.Health;
        Rarity = characterSnapshot.Rarity.ToString();
        Class = characterSnapshot.Class.ToString();
        FirstName = characterSnapshot.FirstName.ToString();
        LastName = characterSnapshot.LastName.ToString();
    }

    public int Level { get; set; }
    public int Health { get; set; }
    public int CurrentHealth { get; set; }
    public string Rarity { get; set; }
    public string Class { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
}
