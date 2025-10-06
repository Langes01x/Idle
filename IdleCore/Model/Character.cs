namespace IdleCore.Model;

public class Character
{
    public int Id { get; set; }

    public required string AccountId { get; set; }
    public Account Account { get; set; } = default!;

    public long Experience { get; set; }
    public RarityEnum Rarity { get; set; }
    public StatEnum Class { get; set; }
    public FirstNameEnum FirstName { get; set; }
    public LastNameEnum LastName { get; set; }
}
