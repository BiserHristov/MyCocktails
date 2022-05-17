namespace MyCocktailsApi.Controllers
{
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Logging;
    using MyCocktailsApi.Data.Models;
    using MyCocktailsApi.Models;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    [ApiController]
    [Route("api/[controller]")]
    public class AccountController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly SignInManager<ApplicationUser> signInManager;
        private readonly ILogger<AccountController> logger;

        public AccountController(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            ILogger<AccountController> logger)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.logger = logger;
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

            var user = new ApplicationUser();

            try
            {
                user = await userManager.FindByEmailAsync(logInModel.Email);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Failed to search for registerd user.");
            }

            if (user == null)
            {
                return NotFound("User doesn't exist!");

            }

            var result = new Microsoft.AspNetCore.Identity.SignInResult();

            try
            {
                result = await signInManager.PasswordSignInAsync(user, logInModel.Password, false, false);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Failed to log in user");
            }

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
