using IdleCore.Helpers;
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
        private readonly ISummonHelper _summonHelper;

        public CharactersController(UserManager<IdentityUser> userManager, IAccountManager accountManager, ISummonHelper summonHelper)
        {
            _userManager = userManager;
            _accountManager = accountManager;
            _summonHelper = summonHelper;
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

            var account = await _accountManager.GetOrCreateAccount(userId, true);

            return View(new CharactersModel { Account = account, SummonCost = _summonHelper.SummonCost });
        }

        // Summon new characters
        public async Task<IActionResult> Summon(int quantity)
        {
            // Authorize attribute should prevent not having a user but redirect to home page if something breaks
            var userId = _userManager.GetUserId(User);
            if (userId is null)
            {
                return RedirectToAction("Index", "Home");
            }

            // Need to load characters at the same time so EF will fix up the navigation property
            var account = await _accountManager.GetOrCreateAccount(userId, true);
            if (account.Diamonds < (quantity * _summonHelper.SummonCost))
            {
                ModelState.AddModelError("quantity", $"Not enough diamonds to summon {quantity} times");
                return BadRequest(ModelState);
            }

            var newCharacters = _summonHelper.SummonCharacters(account, quantity).ToArray();
            await _accountManager.SaveChanges();

            return View("Index", new CharactersModel { Account = account, SummonCost = _summonHelper.SummonCost, NewCharacters = newCharacters });
        }
    }
}
