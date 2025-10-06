using IdleAPI.Models;
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
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;

        public IdleAPIController(UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        // Logout functionality for identity (not implemented by default for some reason)
        [HttpPost("logout")]
        public async Task<IActionResult> Logout()
        {
            // Authorize attribute should prevent not having a user but return 401 if something breaks
            var userId = _userManager.GetUserId(User);
            if (userId is null)
            {
                return Unauthorized();
            }

            await _signInManager.SignOutAsync();
            return Ok();
        }

        // Account deletion
        [HttpPost("delete")]
        public async Task<IActionResult> Delete(DeleteInputModel input)
        {
            // Authorize attribute should prevent not having a user but return 401 if something breaks
            var user = await _userManager.GetUserAsync(User);
            if (user is null)
            {
                return Unauthorized();
            }

            if (!await _userManager.CheckPasswordAsync(user, input.Password))
            {
                ModelState.AddModelError("Password", "Incorrect password.");
                return ValidationProblem();
            }

            var result = await _userManager.DeleteAsync(user);
            if (!result.Succeeded)
            {
                throw new InvalidOperationException($"Unexpected error occurred deleting user.");
            }

            await _signInManager.SignOutAsync();
            return Ok();
        }
    }
}
