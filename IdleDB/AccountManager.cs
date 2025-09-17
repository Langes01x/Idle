using IdleCore;

namespace IdleDB;

public interface IAccountManager
{
    Task<Account> GetOrCreateAccount(string userId);
    Task SaveChanges();
}

public class AccountManager : IAccountManager
{
    private ApplicationDbContext _context;

    public AccountManager(ApplicationDbContext context)
    {
        _context = context;
    }

    /// <summary>
    /// Gets or creates an account for the user using their ID
    /// </summary>
    /// <param name="userId">The user's ID</param>
    /// <returns>The user's account</returns>
    public async Task<Account> GetOrCreateAccount(string userId)
    {
        var account = await _context.Accounts.FindAsync(userId);
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
