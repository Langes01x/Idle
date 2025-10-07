using IdleCore.Model;
using Microsoft.EntityFrameworkCore;

namespace IdleDB.Helpers;

public interface ICharacterManager
{
    Task<Character?> GetCharacter(int id, string userId);
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
