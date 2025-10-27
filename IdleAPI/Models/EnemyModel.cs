using System.Text.Json.Serialization;
using IdleCore.Model;

namespace IdleAPI.Models;

public class EnemyModel
{
    [JsonConstructor]
    private EnemyModel()
    {
        Name = "";
    }

    public EnemyModel(Enemy enemy)
    {
        Id = enemy.Id;
        Name = enemy.Name;
        PhysicalDamage = enemy.PhysicalDamage;
        AetherDamage = enemy.AetherDamage;
        FireDamage = enemy.FireDamage;
        ColdDamage = enemy.ColdDamage;
        PoisonDamage = enemy.PoisonDamage;
        CritRating = enemy.CritRating;
        CritMultiplier = enemy.CritMultiplier;
        ActionSpeed = enemy.ActionSpeed;
        Health = enemy.Health;
        Armour = enemy.Armour;
        Barrier = enemy.Barrier;
        Evasion = enemy.Evasion;
        FireResistance = enemy.FireResistance;
        ColdResistance = enemy.ColdResistance;
        PoisonResistance = enemy.PoisonResistance;
    }

    public int Id { get; set; }

    public string Name { get; set; }

    public decimal PhysicalDamage { get; set; }
    public decimal AetherDamage { get; set; }
    public decimal FireDamage { get; set; }
    public decimal ColdDamage { get; set; }
    public decimal PoisonDamage { get; set; }

    public int CritRating { get; set; }
    public decimal CritMultiplier { get; set; }
    public decimal ActionSpeed { get; set; }

    public int Health { get; set; }
    public int Armour { get; set; }
    public int Barrier { get; set; }
    public decimal Evasion { get; set; }

    public decimal FireResistance { get; set; }
    public decimal ColdResistance { get; set; }
    public decimal PoisonResistance { get; set; }
}
