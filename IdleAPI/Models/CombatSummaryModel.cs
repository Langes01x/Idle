using System.Text.Json.Serialization;
using IdleCore.Model;

namespace IdleAPI.Models;

public class CombatSummaryModel
{
    [JsonConstructor]
    public CombatSummaryModel()
    {
        Result = "";
    }

    public CombatSummaryModel(CombatSummary combatSummary)
    {
        Result = combatSummary.Result.ToString();
    }

    public CharacterSnapshotModel? BackCharacter { get; set; }
    public CharacterSnapshotModel? MiddleCharacter { get; set; }
    public CharacterSnapshotModel? FrontCharacter { get; set; }

    public LevelModel? Level { get; set; }

    public CombatActionModel[]? CombatActions { get; set; }

    public string Result { get; set; }
}
