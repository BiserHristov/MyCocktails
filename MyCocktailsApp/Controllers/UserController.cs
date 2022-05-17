namespace MyCocktailsApi.Controllers
{
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Logging;
    using MyCocktailsApi.Data.Models;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase    // :Controller
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly RoleManager<ApplicationRole> roleManager;
        private readonly ILogger<UserController> logger;

        public UserController(
            UserManager<ApplicationUser> userManager,
            RoleManager<ApplicationRole> roleManager,
            ILogger<UserController> logger)
        {
            this.userManager = userManager;
            this.roleManager = roleManager;
            this.logger = logger;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return Ok("You are at User Index page.");
        }


        [HttpPost("CreateUser")]
        public async Task<IActionResult> Create(User user, bool isAdmin)
        {
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
                logger.LogError(ex, "Failed to create user!");
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
                    logger.LogError(ex, "Failed to add role to user.");
                }

                return Ok("User is created!");
            }
            else
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
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
                logger.LogError(ex, "Failed to create Role.");
            }

            if (result.Succeeded)
            {
                return Ok($"Role {role.Name} is created!");
            }
            else
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }

                errorMessages = ModelState.Values.SelectMany(v => v.Errors.Select(e => e.ErrorMessage)).ToList();

                return BadRequest(string.Join("\n", errorMessages));
            }
        }
    }
}
