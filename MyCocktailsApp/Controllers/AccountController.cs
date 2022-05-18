namespace MyCocktailsApi.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Logging;
    using MyCocktailsApi.Data.Models;
    using MyCocktailsApi.Models;
    using MyCocktailsApi.Services;

    using static ApiConstants.Account;

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
            IUserService userService,
            ILogger<AccountController> logger)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.logger = logger;
        }

        [HttpGet("Index")]
        public IActionResult Index()
        {
            return Ok(AtAccountPageMessage);
        }

        [HttpPost("LogIn")]
        [AllowAnonymous]
        public async Task<IActionResult> Login(InputLoginModel logInModel)
        {
            var logedInUser = await userManager.GetUserAsync(this.User);

            if (logedInUser != null)
            {
                ModelState.AddModelError(string.Empty, AlreadyLoggedMessage);
            }

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
                logger.LogError(ex, FailedSearcForUserMessage);
            }

            if (user == null)
            {
                return NotFound(NotExistingUserMessage);
            }

            var result = new Microsoft.AspNetCore.Identity.SignInResult();

            try
            {
                result = await signInManager.PasswordSignInAsync(user, logInModel.Password, false, false);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, FailedLogInUserMessage);
            }

            if (result.Succeeded)
            {
                return Ok(RedirectAndLogedInMessage + logInModel.Email);
            }
            else
            {
                ModelState.AddModelError(nameof(logInModel.Email), InvalidEmailAndPasswordMessage);
                errorMessages = ModelState.Values.SelectMany(v => v.Errors.Select(e => e.ErrorMessage)).ToList();

                return BadRequest(string.Join("\n", errorMessages));
            }
        }

        [Authorize]
        [HttpGet("LogOut")]
        public async Task<IActionResult> Logout()
        {
            await signInManager.SignOutAsync();
            return Ok(RedirectAndLogedOutMessage);
        }
    }
}
