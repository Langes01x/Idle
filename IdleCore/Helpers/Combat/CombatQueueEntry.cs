namespace IdleCore.Helpers.Combat;

public class CombatQueueEntry
{
    public decimal TimeToNextAction { get; set; }
    public required CombatParticipant CombatParticipant { get; set; }
}
