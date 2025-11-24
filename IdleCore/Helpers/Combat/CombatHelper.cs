using IdleCore.Model;

namespace IdleCore.Helpers.Combat;

public interface ICombatHelper
{
    /// <summary>
    /// Attempt to complete a level.
    /// </summary>
    /// <param name="accountId">ID of the account to associate the combat summary to.</param>
    /// <param name="area">Area of the level being attempted.</param>
    /// <param name="level">Level being attempted.</param>
    /// <param name="party">Party being sent.</param>
    /// <returns>A summary of the combat during the attempt.</returns>
    CombatSummary AttemptLevel(string accountId, Area area, Level level, Party party);
}

public class CombatHelper : ICombatHelper
{
    private readonly ICombatParticipantFactory _combatParticipantFactory;
    private readonly ICharacterSnapshotter _characterSnapshotter;
    private readonly IAttackCalculator _attackCalculator;
    private readonly IDamageApplicator _damageApplicator;

    public CombatHelper(ICombatParticipantFactory combatParticipantFactory,
        ICharacterSnapshotter characterSnapshotter, IAttackCalculator attackCalculator,
        IDamageApplicator damageApplicator)
    {
        _combatParticipantFactory = combatParticipantFactory;
        _characterSnapshotter = characterSnapshotter;
        _attackCalculator = attackCalculator;
        _damageApplicator = damageApplicator;
    }

    public CombatSummary AttemptLevel(string accountId, Area area, Level level, Party party)
    {
        var enemyLevel = (area.Number - 1) * 20 + level.Number;

        var enemies = new List<CombatParticipant>();
        if (level.FrontEnemy != null)
        {
            enemies.Add(_combatParticipantFactory.GetCombatParticipant(level.FrontEnemy, PositionEnum.Front, enemyLevel));
        }
        if (level.MiddleEnemy != null)
        {
            enemies.Add(_combatParticipantFactory.GetCombatParticipant(level.MiddleEnemy, PositionEnum.Middle, enemyLevel));
        }
        if (level.BackEnemy != null)
        {
            enemies.Add(_combatParticipantFactory.GetCombatParticipant(level.BackEnemy, PositionEnum.Back, enemyLevel));
        }

        var characters = new List<CombatParticipant>();
        CharacterSnapshot? frontCharacterSnapshot = null;
        if (party.FrontCharacter != null)
        {
            characters.Add(_combatParticipantFactory.GetCombatParticipant(party.FrontCharacter, PositionEnum.Front));
            frontCharacterSnapshot = _characterSnapshotter.TakeCharacterSnapshot(party.FrontCharacter);
        }
        CharacterSnapshot? middleCharacterSnapshot = null;
        if (party.MiddleCharacter != null)
        {
            characters.Add(_combatParticipantFactory.GetCombatParticipant(party.MiddleCharacter, PositionEnum.Middle));
            middleCharacterSnapshot = _characterSnapshotter.TakeCharacterSnapshot(party.MiddleCharacter);
        }
        CharacterSnapshot? backCharacterSnapshot = null;
        if (party.BackCharacter != null)
        {
            characters.Add(_combatParticipantFactory.GetCombatParticipant(party.BackCharacter, PositionEnum.Back));
            backCharacterSnapshot = _characterSnapshotter.TakeCharacterSnapshot(party.BackCharacter);
        }

        var combatQueue = new CombatQueue(characters.Concat(enemies));
        var combatActions = new List<CombatAction>();
        var time = TimeSpan.Zero;
        var timeLimit = new TimeSpan(0, 1, 0);

        while (true)
        {
            var attackerEntry = combatQueue.Dequeue();

            var secondsPassed = (int)Math.Truncate(attackerEntry.TimeToNextAction);
            var millisecondsPassed = (int)Math.Round((attackerEntry.TimeToNextAction - secondsPassed) * 1000);
            time = time.Add(new TimeSpan(0, 0, 0, secondsPassed, millisecondsPassed));
            if (time > timeLimit)
            {
                return new CombatSummary
                {
                    AccountId = accountId,
                    Level = level,
                    FrontCharacter = frontCharacterSnapshot,
                    MiddleCharacter = middleCharacterSnapshot,
                    BackCharacter = backCharacterSnapshot,
                    CombatActions = combatActions,
                    Result = CombatResultEnum.Draw,
                };
            }

            var defender = attackerEntry.CombatParticipant.Side == SideEnum.Player ? enemies.First(e => !e.IsDead) : characters.First(a => !a.IsDead);
            var attackDamage = _attackCalculator.CalculateAttackDamage(attackerEntry.CombatParticipant, defender);
            _damageApplicator.ApplyDamage(attackDamage, defender);
            if (defender.IsDead)
            {
                combatQueue.Remove(defender);
            }

            combatActions.Add(new CombatAction
            {
                Time = time,
                AttackerSide = attackerEntry.CombatParticipant.Side,
                AttackerPosition = attackerEntry.CombatParticipant.Position,
                DefenderPosition = defender.Position,
                IsDefenderDead = defender.IsDead,

                PhysicalDamageDealt = attackDamage.PhysicalDamageDealt,
                AetherDamageDealt = attackDamage.AetherDamageDealt,
                FireDamageDealt = attackDamage.FireDamageDealt,
                ColdDamageDealt = attackDamage.ColdDamageDealt,
                PoisonDamageDealt = attackDamage.PoisonDamageDealt,
                IsCrit = attackDamage.IsCrit,
                IsMiss = attackDamage.IsMiss,
            });

            if (characters.All(c => c.IsDead))
            {
                return new CombatSummary
                {
                    AccountId = accountId,
                    Level = level,
                    FrontCharacter = frontCharacterSnapshot,
                    MiddleCharacter = middleCharacterSnapshot,
                    BackCharacter = backCharacterSnapshot,
                    CombatActions = combatActions,
                    Result = CombatResultEnum.Lost,
                };
            }

            if (enemies.All(e => e.IsDead))
            {
                return new CombatSummary
                {
                    AccountId = accountId,
                    Level = level,
                    FrontCharacter = frontCharacterSnapshot,
                    MiddleCharacter = middleCharacterSnapshot,
                    BackCharacter = backCharacterSnapshot,
                    CombatActions = combatActions,
                    Result = CombatResultEnum.Won,
                };
            }

            combatQueue.ReAdd(attackerEntry);
        }
    }
}
