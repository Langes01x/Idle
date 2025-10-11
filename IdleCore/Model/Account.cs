namespace IdleCore.Model;

public class Account
{
    public required string Id { get; set; }

    public ICollection<Character> Characters { get; } = default!;
    public ICollection<Party> Parties { get; } = default!;

    public required DateTime LastIdleCollection { get; set; }
    public long Experience { get; set; }
    public long Gold { get; set; }
    public long Diamonds { get; set; }
    public long LevelsCleared { get; set; }
    public bool HasUsedFreePull { get; set; }

    public CharacterSortOrderEnum CharacterSortOrder { get; set; }
}
