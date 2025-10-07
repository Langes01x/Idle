using IdleCore.Model;
using Microsoft.EntityFrameworkCore;

namespace IdleDB.Helpers;

public interface IAccountManager
{
    /// <summary>
    /// Gets or creates an account for the user using their ID
    /// </summary>
    /// <param name="userId">The user's ID</param>
    /// <param name="includeCharacters">Whether to include characters in the query</param>
    /// <returns>The user's account</returns>
    Task<Account> GetOrCreateAccount(string userId, bool includeCharacters = false);

    /// <summary>
    /// Save any changes made to database objects.
    /// </summary>
    /// <returns>A task to be awaited.</returns>
    Task SaveChanges();
}

public class AccountManager : IAccountManager
{
    private readonly ApplicationDbContext _context;

    public AccountManager(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Account> GetOrCreateAccount(string userId, bool includeCharacters = false)
    {
        IQueryable<Account> query = _context.Accounts;
        if (includeCharacters)
        {
            query = query.Include(q => q.Characters);
        }
        var account = await query.Where(q => q.Id == userId).SingleOrDefaultAsync();
        if (account is null)
        {
            account = new Account { Id = userId, LastIdleCollection = DateTime.UtcNow };
            await _context.Accounts.AddAsync(account);
            await _context.SaveChangesAsync();
        }

        return account;
    }

    public async Task SaveChanges()
    {
        await _context.SaveChangesAsync();
    }
}
