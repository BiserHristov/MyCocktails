namespace MyCocktailsApi.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Logging;
    using MyCocktailsApi.Data.Models;
    using MyCocktailsApi.Services;

    using static ApiConstants.User;

    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase 
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly RoleManager<ApplicationRole> roleManager;
        private readonly IUserService userService;
        private readonly ILogger<UserController> logger;

        public UserController(
            UserManager<ApplicationUser> userManager,
            RoleManager<ApplicationRole> roleManager,
            IUserService userService,
            ILogger<UserController> logger)
        {
            this.userManager = userManager;
            this.roleManager = roleManager;
            this.userService = userService;
            this.logger = logger;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return Ok(AtUserIndexPageMessage);
        }

        [HttpPost("CreateUser")]
        public async Task<IActionResult> Create(User user, bool isAdmin)
        {
            var logedInUser = new ApplicationUser();
            try
            {
                logedInUser = await userManager.GetUserAsync(this.User);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, UnableCheckForUserMessage);
            }

            if (logedInUser != null)
            {
                ModelState.AddModelError(string.Empty, UserAlreadyLoggedMessage);
            }

            bool userExist = false;

            try
            {
                userExist = await userService.UserExist(user.Email);
            }
            catch (Exception ex )
            {
                logger.LogError(ex, FailedSearchForUserMessage);
            }

            if (userExist)
            {
                ModelState.AddModelError(string.Empty, AlreadyExistingUserMessage);
            }

            List<string> errorMessages = new List<string>();

            if (!ModelState.IsValid)
            {
                errorMessages = ModelState.Values.SelectMany(v => v.Errors.Select(e => e.ErrorMessage)).ToList();
                return BadRequest(string.Join("\n", errorMessages));
            }

            var appUser = new ApplicationUser
            {
                UserName = user.Name,
                Email = user.Email
            };

            var result = new IdentityResult();

            try
            {
                result = await userManager.CreateAsync(appUser, user.Password);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, FailCreateUserMessage);
            }

            if (result.Succeeded)
            {
                string role = isAdmin ? "Admin" : "User";
                try
                {
                    await userManager.AddToRoleAsync(appUser, role);
                }
                catch (Exception ex)
                {
                    logger.LogError(ex, FailAddRoleToUserMessage);
                }

                return Ok(SuccessfullyUserCreateMessage);
            }
            else
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }

                errorMessages = ModelState.Values.SelectMany(v => v.Errors.Select(e => e.ErrorMessage)).ToList();

                return BadRequest(string.Join("\n", errorMessages));
            }
        }

        [HttpPost("CreateRole")]
        public async Task<IActionResult> CreateRole(UserRole userRole)
        {
            List<string> errorMessages = new List<string>();

            if (!ModelState.IsValid)
            {
                errorMessages = ModelState.Values.SelectMany(v => v.Errors.Select(e => e.ErrorMessage)).ToList();

                return BadRequest(string.Join("\n", errorMessages));
            }

            var role = new ApplicationRole()
            {
                Name = userRole.Name
            };

            var result = new IdentityResult();

            try
            {
                result = await roleManager.CreateAsync(role);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, FailAddRoleMessage);
            }

            if (result.Succeeded)
            {
                return Ok(SuccessfullyRoleCreateMessage);
            }
            else
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }

                errorMessages = ModelState.Values.SelectMany(v => v.Errors.Select(e => e.ErrorMessage)).ToList();

                return BadRequest(string.Join("\n", errorMessages));
            }
        }
    }
}
