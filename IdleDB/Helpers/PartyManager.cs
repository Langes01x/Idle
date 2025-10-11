using IdleCore.Model;
using Microsoft.EntityFrameworkCore;

namespace IdleDB.Helpers;

public interface IPartyManager
{
    /// <summary>
    /// Gets a collection of all parties for the user's account.
    /// </summary>
    /// <param name="userId">Account ID.</param>
    /// <returns>The account's parties.</returns>
    Task<Party[]> GetParties(string userId);

    /// <summary>
    /// Gets a party if it exists on the user's account.
    /// </summary>
    /// <param name="id">Party ID.</param>
    /// <param name="userId">Account ID.</param>
    /// <returns>The party if it exists.</returns>
    Task<Party?> GetParty(int id, string userId);

    /// <summary>
    /// Creates a new party.
    /// </summary>
    /// <param name="party">The party to create.</param>
    void CreateParty(Party party);

    /// <summary>
    /// Delete a party.
    /// </summary>
    /// <param name="party">The party to delete.</param>
    void DeleteParty(Party party);

    /// <summary>
    /// Save any changes made to database objects.
    /// </summary>
    /// <returns>A task to be awaited.</returns>
    Task SaveChanges();
}

public class PartyManager : IPartyManager
{
    private readonly ApplicationDbContext _context;

    public PartyManager(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Party[]> GetParties(string userId)
    {
        return await _context.Parties.Where(p => p.AccountId == userId)
            .Include(p => p.BackCharacter)
            .Include(p => p.MiddleCharacter)
            .Include(p => p.FrontCharacter)
            .ToArrayAsync();
    }

    public async Task<Party?> GetParty(int id, string userId)
    {
        return await _context.Parties.SingleOrDefaultAsync(c => c.Id == id && c.AccountId == userId);
    }

    public void CreateParty(Party party)
    {
        _context.Parties.Add(party);
    }

    public void DeleteParty(Party party)
    {
        _context.Parties.Remove(party);
    }

    public async Task SaveChanges()
    {
        await _context.SaveChangesAsync();
    }
}
