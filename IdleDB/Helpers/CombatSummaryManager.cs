using IdleCore.Model;
using Microsoft.EntityFrameworkCore;

namespace IdleDB.Helpers;

public interface ICombatSummaryManager
{
    /// <summary>
    /// Gets combat summaries from past attempts on a level.
    /// </summary>
    /// <param name="levelId">Level ID.</param>
    /// <param name="userId">Account ID.</param>
    /// <returns>Combat summaries.</returns>
    Task<CombatSummary[]> GetCombatSummaries(int levelId, string userId);

    /// <summary>
    /// Add a combat summary to the database.
    /// </summary>
    /// <param name="combatSummary">Combat summary to add.</param>
    void AddCombatSummary(CombatSummary combatSummary);

    /// <summary>
    /// Save any changes made to database objects.
    /// </summary>
    /// <returns>A task to be awaited.</returns>
    Task SaveChanges();
}

public class CombatSummaryManager : ICombatSummaryManager
{
    private readonly ApplicationDbContext _context;

    public CombatSummaryManager(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<CombatSummary[]> GetCombatSummaries(int levelId, string userId)
    {
        return await _context.CombatSummaries
            .Where(s => s.LevelId == levelId && s.AccountId == userId)
            .OrderByDescending(s => s.Id)
            .ToArrayAsync();
    }

    public void AddCombatSummary(CombatSummary combatSummary)
    {
        _context.CombatSummaries.Add(combatSummary);
    }

    public async Task SaveChanges()
    {
        await _context.SaveChangesAsync();
    }
}
