using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace IdleAPI.Controllers
{
    [Authorize]
    [ApiController]
    [Route("")]
    public class IdleAPIController : ControllerBase
    {
        private readonly SignInManager<IdentityUser> _signInManager;

        public IdleAPIController(SignInManager<IdentityUser> signInManager)
        {
            _signInManager = signInManager;
        }

        // Logout functionality for identity (not implemented by default for some reason)
        [HttpPost("logout")]
        public async Task Logout()
        {
            await _signInManager.SignOutAsync();
        }
    }
}
