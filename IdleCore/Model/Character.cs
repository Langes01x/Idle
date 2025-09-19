namespace IdleCore.Model;

public class Character
{
    public int Id { get; set; }
    public required string AccountId { get; set; }
    public Account Account { get; set; } = default!;

    public int Level { get; set; }
    public RarityEnum Rarity { get; set; }
    public StatEnum Class { get; set; }
    public FirstNameEnum FirstName { get; set; }
    public LastNameEnum LastName { get; set; }

    public int Strength => (Class.HasFlag(StatEnum.Strength) ? 2 : 0) * Level * (int)Rarity;
    public int Intelligence => (Class.HasFlag(StatEnum.Intelligence) ? 2 : 0) * Level * (int)Rarity;
    public int Dexterity => (Class.HasFlag(StatEnum.Dexterity) ? 2 : 1) * Level * (int)Rarity;
    public int Vitality => (Class.HasFlag(StatEnum.Vitality) ? 2 : 1) * Level * (int)Rarity;
    public int Constitution => (Class.HasFlag(StatEnum.Constitution) ? 2 : 1) * Level * (int)Rarity;
    public int Wisdom => (Class.HasFlag(StatEnum.Wisdom) ? 2 : 1) * Level * (int)Rarity;
}
