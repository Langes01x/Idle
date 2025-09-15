using IdleUI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace IdleUI.Controllers
{
    [Authorize]
    public class GameController : Controller
    {
        private readonly UserManager<IdentityUser> _userManager;

        public GameController(UserManager<IdentityUser> userManager)
        {
            _userManager = userManager;
        }

        // GET: GameController
        public async Task<IActionResult> Index()
        {
            // Authorize attribute should prevent not having a user but redirect to home page if something breaks
            var user = await _userManager.GetUserAsync(User);
            if (user is null)
            {
                return RedirectPreserveMethod("./");
            }

            var id = user.Id;
            return View(new GameModel { Id = id });
        }

    }
}
