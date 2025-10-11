using System.Text.Json.Serialization;
using IdleCore.Model;

namespace IdleAPI.Models;

public class PartyModel
{
    [JsonConstructor]
    private PartyModel()
    {
    }

    public PartyModel(Party party)
    {
        Id = party.Id;
        Name = party.Name;
    }

    public int Id { get; set; }
    public string? Name { get; set; }

    public CharacterModel? BackCharacter { get; set; }
    public CharacterModel? MiddleCharacter { get; set; }
    public CharacterModel? FrontCharacter { get; set; }
}
