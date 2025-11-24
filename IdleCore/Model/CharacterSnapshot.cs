namespace IdleCore.Model;

public class CharacterSnapshot
{
    public int Id { get; set; }

    public int Level { get; set; }
    public int Health { get; set; }
    public RarityEnum Rarity { get; set; }
    public StatEnum Class { get; set; }
    public FirstNameEnum FirstName { get; set; }
    public LastNameEnum LastName { get; set; }
}
