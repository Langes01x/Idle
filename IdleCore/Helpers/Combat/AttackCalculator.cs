namespace IdleCore.Helpers.Combat;

public interface IAttackCalculator
{
    /// <summary>
    /// Calculate the damage of the attack.
    /// </summary>
    /// <param name="attacker">The attacker.</param>
    /// <param name="defender">The defender.</param>
    /// <returns>The damage dealt by the attacker to the defender.</returns>
    AttackDamage CalculateAttackDamage(CombatParticipant attacker, CombatParticipant defender);
}

public class AttackCalculator : IAttackCalculator
{
    private readonly Random _rng;
    private readonly IDamageCalculator _damageCalculator;

    public AttackCalculator(Random rng, IDamageCalculator damageCalculator)
    {
        _rng = rng;
        _damageCalculator = damageCalculator;
    }

    public AttackDamage CalculateAttackDamage(CombatParticipant attacker, CombatParticipant defender)
    {
        var evasionRoll = (decimal)_rng.NextDouble();
        if (evasionRoll < defender.Evasion)
        {
            return new AttackDamage
            {
                IsMiss = true,
            };
        }

        var critRoll = (decimal)_rng.NextDouble();
        var isCrit = critRoll < attacker.CritChance;

        var levelDifference = attacker.Level - defender.Level;
        var cappedLevelDifference = Math.Min(Math.Max(levelDifference, -10), 10);
        var levelEffect = cappedLevelDifference / 10m;

        return new AttackDamage
        {
            PhysicalDamageDealt = _damageCalculator.CalculateDamage(attacker.PhysicalDamage, defender.ArmourEffect, levelEffect, isCrit, attacker.CritMultiplier),
            AetherDamageDealt = _damageCalculator.CalculateDamage(attacker.AetherDamage, defender.BarrierEffect, levelEffect, isCrit, attacker.CritMultiplier),
            FireDamageDealt = _damageCalculator.CalculateDamage(attacker.FireDamage, defender.FireResistance, levelEffect, isCrit, attacker.CritMultiplier),
            ColdDamageDealt = _damageCalculator.CalculateDamage(attacker.ColdDamage, defender.ColdResistance, levelEffect, isCrit, attacker.CritMultiplier),
            PoisonDamageDealt = _damageCalculator.CalculateDamage(attacker.PoisonDamage, defender.PoisonResistance, levelEffect, isCrit, attacker.CritMultiplier),
            IsCrit = isCrit,
        };
    }
}
