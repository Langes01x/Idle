namespace IdleCore.Model;

public class Party
{
    public int Id { get; set; }

    public required string AccountId { get; set; }

    public string? Name { get; set; }

    public int? BackCharacterId { get; set; }
    public Character? BackCharacter { get; set; }

    public int? MiddleCharacterId { get; set; }
    public Character? MiddleCharacter { get; set; }

    public int? FrontCharacterId { get; set; }
    public Character? FrontCharacter { get; set; }
}
