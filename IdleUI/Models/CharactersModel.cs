using IdleCore.Model;

namespace IdleUI.Models;

public class CharactersModel
{
    public required Account Account { get; set; }
    public ICollection<Character>? NewCharacters { get; set; }
    public required int SummonCost { get; set; }
}
