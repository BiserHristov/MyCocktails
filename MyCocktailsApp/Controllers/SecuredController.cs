namespace MyCocktailsApi.Controllers
{
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    using static ApiConstants.Secured;

    [Route("api/[controller]")]
    [ApiController]

    public class SecuredController : ControllerBase
    {
        [HttpGet("User")]
        public IActionResult UserPage()
        {
            return Ok(AtUserPageMessage);
        }

        [HttpGet("Admin")]
        [Authorize(Roles = "Admin")]
        public IActionResult AdminPage()
        {
            return Ok(AtAdminPageMessage);
        }
    }
}
