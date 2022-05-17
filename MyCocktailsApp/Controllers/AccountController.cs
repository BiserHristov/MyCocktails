namespace MyCocktailsApi.Controllers
{
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using MyCocktailsApi.Data.Models;
    using MyCocktailsApi.Models;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    [ApiController]
    [Route("api/[controller]")]
    public class AccountController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly SignInManager<ApplicationUser> signInManager;

        public AccountController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
        }

        [HttpGet("Index")]
        public IActionResult Index()
        {
            return Ok("You are at Account Index page.");
        }

        [HttpPost("LogIn")]
        [AllowAnonymous]
        public async Task<IActionResult> Login(LoginInputModel logInModel)
        {
            List<string> errorMessages = new List<string>();

            if (!ModelState.IsValid)
            {
                errorMessages = ModelState.Values.SelectMany(v => v.Errors.Select(e => e.ErrorMessage)).ToList();

                return BadRequest(string.Join("\n", errorMessages));
            }

            var user = await userManager.FindByEmailAsync(logInModel.Email);

            if (user == null)
            {
                return NotFound("User doesn't exist!");

            }

            var result = await signInManager.PasswordSignInAsync(user, logInModel.Password, false, false);

            if (result.Succeeded)
            {
                return Ok($"You are redirected and logged In as {logInModel.Email}!");
            }
            else
            {

                ModelState.AddModelError(nameof(logInModel.Email), "Invalid email or password!");
                errorMessages = ModelState.Values.SelectMany(v => v.Errors.Select(e => e.ErrorMessage)).ToList();

                return BadRequest(string.Join("\n", errorMessages));
            }

        }

        [Authorize]
        [HttpGet("LogOut")]
        public async Task<IActionResult> Logout()
        {
            await signInManager.SignOutAsync();
            return Ok("You are redirected and logged Out!");
        }
    }
}
