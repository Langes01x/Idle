namespace IdleCore.Helpers.Combat;

public interface IDamageCalculator
{
    /// <summary>
    /// Calculate the damage for part of the attack.
    /// </summary>
    /// <param name="initialDamage">Damage before mitigation and crit.</param>
    /// <param name="defenceEffect">Effect of the defence.</param>
    /// <param name="levelEffect">Effect of the level discrepancy.</param>
    /// <param name="isCrit">Whether or not the attack was a crit.</param>
    /// <param name="critMultiplier">Multiplier for critical damage.</param>
    /// <returns></returns>
    decimal CalculateDamage(decimal initialDamage, decimal defenceEffect, decimal levelEffect, bool isCrit, decimal critMultiplier);
}

public class DamageCalculator : IDamageCalculator
{
    public decimal CalculateDamage(decimal initialDamage, decimal defenceEffect, decimal damageEffectiveness, bool isCrit, decimal critMultiplier)
    {
        var damage = initialDamage * (1m - defenceEffect) * (1m - damageEffectiveness);
        if (isCrit)
        {
            damage *= critMultiplier;
        }

        return damage;
    }
}
