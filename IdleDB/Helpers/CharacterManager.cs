using IdleCore.Model;
using Microsoft.EntityFrameworkCore;

namespace IdleDB.Helpers;

public interface ICharacterManager
{
    /// <summary>
    /// Gets a character if it exists on the user's account.
    /// </summary>
    /// <param name="id">Character ID.</param>
    /// <param name="userId">Account ID.</param>
    /// <returns>The character if it exists.</returns>
    Task<Character?> GetCharacter(int id, string userId);

    /// <summary>
    /// Save any changes made to database objects.
    /// </summary>
    /// <returns>A task to be awaited.</returns>
    Task SaveChanges();
}

public class CharacterManager : ICharacterManager
{
    private readonly ApplicationDbContext _context;

    public CharacterManager(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Character?> GetCharacter(int id, string userId)
    {
        return await _context.Characters.SingleOrDefaultAsync(c => c.Id == id && c.AccountId == userId);
    }

    public async Task SaveChanges()
    {
        await _context.SaveChangesAsync();
    }
}
