namespace IdleCore.Helpers.Combat;

public interface IDamageApplicator
{
    /// <summary>
    /// Apply attack damage to a defender.
    /// </summary>
    /// <param name="attackDamage">Attack damage to be dealt.</param>
    /// <param name="defender">The defender to apply it to.</param>
    void ApplyDamage(AttackDamage attackDamage, CombatParticipant defender);
}

public class DamageApplicator : IDamageApplicator
{
    public void ApplyDamage(AttackDamage attackDamage, CombatParticipant defender)
    {
        if (attackDamage.IsMiss)
        {
            return;
        }

        defender.CurrentHealth -= attackDamage.PhysicalDamageDealt;
        defender.CurrentHealth -= attackDamage.AetherDamageDealt;
        defender.CurrentHealth -= attackDamage.FireDamageDealt;
        defender.CurrentHealth -= attackDamage.ColdDamageDealt;
        defender.CurrentHealth -= attackDamage.PoisonDamageDealt;

        if (defender.CurrentHealth < 0m)
        {
            defender.IsDead = true;
        }
    }
}
