namespace MyCocktailsApi.Controllers
{
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    [Route("api/[controller]")]
    [ApiController]

    public class SecuredController : ControllerBase
    {
        [HttpGet("User")]
        public IActionResult UserPage()
        {
            return Ok("You are at page for Users.");
        }

        [HttpGet("Admin")]
        [Authorize(Roles = "Admin")]
        public IActionResult AdminPage()
        {
            return Ok("You are at page for Admins.");
        }
    }
}
