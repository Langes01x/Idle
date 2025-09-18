using IdleDB;
using IdleUI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace IdleUI.Controllers
{
    [Authorize]
    public class CharactersController : Controller
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IAccountManager _accountManager;

        public CharactersController(UserManager<IdentityUser> userManager, IAccountManager accountManager)
        {
            _userManager = userManager;
            _accountManager = accountManager;
        }

        // Display a list of characters on your account.
        public async Task<IActionResult> Index()
        {
            // Authorize attribute should prevent not having a user but redirect to home page if something breaks
            var userId = _userManager.GetUserId(User);
            if (userId is null)
            {
                return RedirectToAction("Index", "Home");
            }

            var characters = await _accountManager.GetCharacters(userId);

            return View(new CharactersModel { Characters = characters });
        }
    }
}
