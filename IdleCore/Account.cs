namespace IdleCore;

public class Account
{
    public required string Id { get; set; }
    public required DateTime LastIdleCollection { get; set; }
    public long Experience { get; set; }
    public long Gold { get; set; }
}
