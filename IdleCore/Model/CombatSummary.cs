namespace IdleCore.Model;

public class CombatSummary
{
    public int Id { get; set; }

    public required string AccountId { get; set; }

    public int LevelId { get; set; }
    public required Level Level { get; set; }

    public int? FrontCharacterId { get; set; }
    public CharacterSnapshot? FrontCharacter { get; set; }

    public int? MiddleCharacterId { get; set; }
    public CharacterSnapshot? MiddleCharacter { get; set; }

    public int? BackCharacterId { get; set; }
    public CharacterSnapshot? BackCharacter { get; set; }

    public ICollection<CombatAction> CombatActions { get; set; } = default!;

    public CombatResultEnum Result { get; set; }
}
