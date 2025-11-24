using IdleCore.Model;

namespace IdleCore.Helpers.Combat;

public interface ICombatParticipantFactory
{
    /// <summary>
    /// Get a combat participant for the enemy.
    /// </summary>
    /// <param name="enemy">The enemy.</param>
    /// <param name="position">Position of the enemy.</param>
    /// <param name="level">Level of the enemy.</param>
    /// <returns>The enemy combat participant.</returns>
    CombatParticipant GetCombatParticipant(Enemy enemy, PositionEnum position, int level);

    /// <summary>
    /// Get a combat participant for the character.
    /// </summary>
    /// <param name="character">The character.</param>
    /// <param name="position">Position of the character.</param>
    /// <returns>The character combat participant.</returns>
    CombatParticipant GetCombatParticipant(Character character, PositionEnum position);
}

public class CombatParticipantFactory : ICombatParticipantFactory
{
    private readonly IStatCalculator _statCalculator;
    private readonly ILevelCalculator _levelCalculator;

    public CombatParticipantFactory(IStatCalculator statCalculator, ILevelCalculator levelCalculator)
    {
        _statCalculator = statCalculator;
        _levelCalculator = levelCalculator;
    }

    public CombatParticipant GetCombatParticipant(Enemy enemy, PositionEnum position, int level)
    {
        return new CombatParticipant
        {
            Position = position,
            Side = SideEnum.Enemy,
            IsDead = false,

            Level = level,

            PhysicalDamage = enemy.PhysicalDamage,
            AetherDamage = enemy.AetherDamage,
            FireDamage = enemy.FireDamage,
            ColdDamage = enemy.ColdDamage,
            PoisonDamage = enemy.PoisonDamage,

            CritChance = _statCalculator.CalculateCritChance(enemy.CritRating),
            CritMultiplier = enemy.CritMultiplier,
            ActionSpeed = enemy.ActionSpeed,

            StartingHealth = enemy.Health,
            CurrentHealth = enemy.Health,
            ArmourEffect = _statCalculator.CalculateDefenceEffect(enemy.Armour),
            BarrierEffect = _statCalculator.CalculateDefenceEffect(enemy.Barrier),
            Evasion = enemy.Evasion,

            FireResistance = enemy.FireResistance,
            ColdResistance = enemy.ColdResistance,
            PoisonResistance = enemy.PoisonResistance,
        };
    }

    public CombatParticipant GetCombatParticipant(Character character, PositionEnum position)
    {
        var level = _levelCalculator.GetLevel(character);
        var vitality = _statCalculator.CalculateVitality(character, level);
        var health = _statCalculator.CalculateHealth(character, vitality);
        return new CombatParticipant
        {
            Position = position,
            Side = SideEnum.Player,
            IsDead = false,

            Level = level,

            PhysicalDamage = _statCalculator.CalculatePhysicalDamage(character, _statCalculator.CalculateStrength(character, level)),
            AetherDamage = _statCalculator.CalculateAetherDamage(character, _statCalculator.CalculateIntelligence(character, level)),
            FireDamage = 0,
            ColdDamage = 0,
            PoisonDamage = 0,

            CritChance = _statCalculator.CalculateCritChance(_statCalculator.CalculateCritRating(character, _statCalculator.CalculateDexterity(character, level))),
            CritMultiplier = _statCalculator.CalculateCritMultiplier(character),
            ActionSpeed = _statCalculator.CalculateActionSpeed(character),

            StartingHealth = health,
            CurrentHealth = health,
            ArmourEffect = _statCalculator.CalculateDefenceEffect(_statCalculator.CalculateArmour(character, _statCalculator.CalculateConstitution(character, level))),
            BarrierEffect = _statCalculator.CalculateDefenceEffect(_statCalculator.CalculateBarrier(character, _statCalculator.CalculateWisdom(character, level))),
            Evasion = _statCalculator.CalculateEvasion(character),

            FireResistance = _statCalculator.CalculateFireResistance(character),
            ColdResistance = _statCalculator.CalculateColdResistance(character),
            PoisonResistance = _statCalculator.CalculatePoisonResistance(character),
        };
    }
}
