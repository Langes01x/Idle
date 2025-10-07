using IdleAPI.Models;
using IdleCore.Helpers;
using IdleDB.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace IdleAPI.Controllers
{
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class AccountController : ControllerBase
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IAccountManager _accountManager;
        private readonly ICollectionHelper _collectionHelper;

        public AccountController(UserManager<IdentityUser> userManager, IAccountManager accountManager,
            ICollectionHelper collectionHelper)
        {
            _userManager = userManager;
            _accountManager = accountManager;
            _collectionHelper = collectionHelper;
        }

        // Get account info.
        [HttpGet]
        public async Task<ActionResult<AccountModel>> Get()
        {
            // Authorize attribute should prevent not having a user but return 401 if something breaks
            var userId = _userManager.GetUserId(User);
            if (userId is null)
            {
                return Unauthorized();
            }

            var account = await _accountManager.GetOrCreateAccount(userId);

            return new AccountModel(account, _userManager.GetUserName(User)!, _collectionHelper);
        }

        // Collect idle rewards.
        [HttpPost("Collect")]
        public async Task<ActionResult<AccountModel>> Collect()
        {
            // Authorize attribute should prevent not having a user but return 401 if something breaks
            var userId = _userManager.GetUserId(User);
            if (userId is null)
            {
                return Unauthorized();
            }

            var account = await _accountManager.GetOrCreateAccount(userId);
            _collectionHelper.CollectIdleRewards(account);
            await _accountManager.SaveChanges();

            return new AccountModel(account, _userManager.GetUserName(User)!, _collectionHelper);
        }
    }
}
