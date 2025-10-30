using IdleCore.Model;
using Microsoft.EntityFrameworkCore;

namespace IdleDB.Helpers;

public interface IAreaManager
{
    /// <summary>
    /// Gets a collection of all areas in the game.
    /// </summary>
    /// <returns>The game's areas.</returns>
    Task<Area[]> GetAreas();

    /// <summary>
    /// Gets detailed information about a level.
    /// </summary>
    /// <param name="areaId">Area ID.</param>
    /// <param name="levelId">Level ID.</param>
    /// <returns>The level if it exists.</returns>
    Task<Level?> GetLevelDetails(int areaId, int levelId);
}

public class AreaManager : IAreaManager
{
    private readonly ApplicationDbContext _context;

    public AreaManager(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Area[]> GetAreas()
    {
        return await _context.Areas
            .Include(p => p.Levels)
            .ToArrayAsync();
    }

    public async Task<Level?> GetLevelDetails(int areaId, int levelId)
    {
        return await _context.Levels
            .Include(l => l.BackEnemy)
            .Include(l => l.MiddleEnemy)
            .Include(l => l.FrontEnemy)
            .SingleOrDefaultAsync(l => l.Id == levelId && l.AreaId == areaId);
    }
}
