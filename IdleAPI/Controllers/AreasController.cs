using IdleAPI.Models;
using IdleDB.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace IdleAPI.Controllers;

[Authorize]
[ApiController]
[Route("[controller]")]
public class AreasController : ControllerBase
{
    private readonly IAreaManager _areaManager;

    public AreasController(IAreaManager areaManager)
    {
        _areaManager = areaManager;
    }

    // Display a list of areas.
    [HttpGet]
    public async Task<ActionResult<AreaModel[]>> Get()
    {
        return (await _areaManager.GetAreas())
            .Select(a => new AreaModel(a)
            {
                Levels = a.Levels.Select(l => new LevelModel(l)).ToArray(),
            })
            .ToArray();
    }
}
