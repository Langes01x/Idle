using System.Diagnostics;
using IdleUI.Models;
using Microsoft.AspNetCore.Mvc;

namespace IdleUI.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;

    public HomeController(ILogger<HomeController> logger)
    {
        _logger = logger;
    }

    // Display the home page.
    [HttpGet]
    public IActionResult Index()
    {
        return View();
    }

    // Display the privacy page.
    [HttpGet]
    public IActionResult Privacy()
    {
        return View();
    }

    // Display the error page.
    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
