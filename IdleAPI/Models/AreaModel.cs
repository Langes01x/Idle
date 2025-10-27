using System.Text.Json.Serialization;
using IdleCore.Model;

namespace IdleAPI.Models;

public class AreaModel
{
    [JsonConstructor]
    private AreaModel()
    {
        Name = "";
    }

    public AreaModel(Area area)
    {
        Id = area.Id;
        Number = area.Number;
        Name = area.Name;
    }

    public int Id { get; set; }

    public int Number { get; set; }
    public string Name { get; set; }

    public LevelModel[]? Levels { get; set; }
}
