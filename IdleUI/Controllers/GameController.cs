using IdleDB.Helpers;
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
        private readonly IAccountManager _accountManager;

        public GameController(UserManager<IdentityUser> userManager, IAccountManager accountManager)
        {
            _userManager = userManager;
            _accountManager = accountManager;
        }

        // Display the main game screen.
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            // Authorize attribute should prevent not having a user but redirect to home page if something breaks
            var userId = _userManager.GetUserId(User);
            if (userId is null)
            {
                return RedirectToAction("Index", "Home");
            }

            var account = await _accountManager.GetOrCreateAccount(userId);

            return View(new GameModel { Account = account });
        }

        // Collect idle rewards.
        [HttpPost]
        public async Task<IActionResult> Collect()
        {
            // Authorize attribute should prevent not having a user but redirect to home page if something breaks
            var userId = _userManager.GetUserId(User);
            if (userId is null)
            {
                return RedirectToAction("Index", "Home");
            }

            var account = await _accountManager.GetOrCreateAccount(userId);
            account.CollectIdleRewards();
            await _accountManager.SaveChanges();

            return RedirectToAction("Index");
        }
    }
}
