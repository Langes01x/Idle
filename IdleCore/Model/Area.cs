namespace IdleCore.Model;

public class Area
{
    public int Id { get; set; }

    public int Number { get; set; }
    public required string Name { get; set; }

    public ICollection<Level> Levels { get; set; } = default!;
}
