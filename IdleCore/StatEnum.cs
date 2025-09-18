namespace IdleCore;

[Flags]
public enum StatEnum
{
    // Base stats
    Strength = 1,
    Intelligence = 2,
    Dexterity = 4,
    Vitality = 8,
    Constitution = 16,
    Wisdom = 32,

    // Classes
    Paladin = Intelligence | Constitution | Wisdom,
    Bulwark = Strength | Constitution | Wisdom,
    Runemaster = Intelligence | Constitution | Dexterity,
    Samurai = Strength | Constitution | Dexterity,
    Cleric = Intelligence | Constitution | Vitality,
    Knight = Strength | Constitution | Vitality,
    Wizard = Intelligence | Dexterity | Wisdom,
    Monk = Strength | Dexterity | Wisdom,
    Bishop = Intelligence | Vitality | Wisdom,
    Spellblade = Strength | Vitality | Wisdom,
    Bard = Intelligence | Vitality | Dexterity,
    Rogue = Strength | Vitality | Dexterity,
}
