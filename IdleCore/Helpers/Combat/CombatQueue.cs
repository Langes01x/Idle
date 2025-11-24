namespace IdleCore.Helpers.Combat;

public class CombatQueue
{
    private readonly List<CombatQueueEntry> _participants;

    public CombatQueue(IEnumerable<CombatParticipant> combatParticipants)
    {
        _participants = combatParticipants
            .OrderBy(p => p.ActionSpeed)
            .Select(p => new CombatQueueEntry { TimeToNextAction = Math.Round(p.ActionSpeed, 3), CombatParticipant = p })
            .ToList();
    }

    public CombatQueueEntry Dequeue()
    {
        var next = _participants[0];
        _participants.RemoveAt(0);
        foreach (var entry in _participants)
        {
            entry.TimeToNextAction -= next.TimeToNextAction;
        }
        return next;
    }

    public void ReAdd(CombatQueueEntry entry)
    {
        entry.TimeToNextAction = Math.Round(entry.CombatParticipant.ActionSpeed, 3);
        for (int i = 0; i < _participants.Count; i++)
        {
            if (_participants[i].TimeToNextAction > entry.TimeToNextAction)
            {
                _participants.Insert(i, entry);
                return;
            }
        }
        _participants.Add(entry);
    }

    public void Remove(CombatParticipant combatParticipant)
    {
        for (int i = 0; i < _participants.Count; i++)
        {
            if (_participants[i].CombatParticipant == combatParticipant)
            {
                _participants.RemoveAt(i);
                return;
            }
        }
    }
}
