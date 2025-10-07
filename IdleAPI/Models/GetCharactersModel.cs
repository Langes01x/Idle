using IdleCore.Model;

namespace IdleAPI.Models;

public class GetCharactersModel
{
    public required CharacterModel[] Characters { get; set; }
    public required int SummonCost { get; set; }
    public required Dictionary<string, CharacterSortOrderEnum> SortOrderOptions { get; set; }
    public required CharacterSortOrderEnum DefaultSortOrder { get; set; }
}
