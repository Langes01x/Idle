using IdleCore.Model;

namespace IdleCore.Helpers;

public interface ICharacterSorter
{
    /// <summary>
    /// Sort characters based on a chosen sort order.
    /// </summary>
    /// <param name="characters">The characters to sort.</param>
    /// <param name="sortOrder">The order to sort them in.</param>
    /// <returns>The sorted characters.</returns>
    IEnumerable<Character> SortCharacters(IEnumerable<Character> characters, CharacterSortOrderEnum sortOrder);
}

public class CharacterSorter : ICharacterSorter
{
    public IEnumerable<Character> SortCharacters(IEnumerable<Character> characters, CharacterSortOrderEnum sortOrder)
    {
        return sortOrder switch
        {
            CharacterSortOrderEnum.Acquisition => characters.OrderBy(c => c.Id),
            CharacterSortOrderEnum.Level => characters.OrderByDescending(c => c.Experience),
            CharacterSortOrderEnum.Rarity => characters.OrderByDescending(c => c.Rarity),
            _ => characters,
        };
    }
}
