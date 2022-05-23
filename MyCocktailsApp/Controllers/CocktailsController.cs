namespace MyCocktailsApi.Controllers
{
    using AutoMapper;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using MyCocktailsApi.Models;
    using MyCocktailsApi.Services;
    using System;
    using System.Linq;
    using System.Security.Claims;
    using System.Threading.Tasks;
    using static ApiConstants.Cocktail;

    [Route("api/[controller]")]
    [ApiController]
    public class CocktailsController : ControllerBase
    {
        private readonly ICocktailService cocktailService;
        private readonly IMapper mapper;
        public Func<string> GetUserId;
        public CocktailsController(ICocktailService cocktailService, IMapper mapper)
        {
            this.cocktailService = cocktailService;
            this.mapper = mapper;
            GetUserId = () => User.FindFirst(ClaimTypes.Email).Value;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var outputCocktails = await cocktailService.GetAllAsync();

            if (outputCocktails == null || !outputCocktails.Any())
            {
                return NotFound(NotExistingOrEmptyCollectionMessage);
            }

            return Ok(outputCocktails);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(string id)
        {
            if (id == null)
            {
                return BadRequest(RequiredIdMessage);
            }

            var outputCocktail = await cocktailService.GetByIdAsync(id);

            if (outputCocktail == null)
            {
                return NotFound(NotExistingCocktailMessage);
            }

            return Ok(outputCocktail);
        }

        [HttpGet("name/{name}")]
        public async Task<IActionResult> GetByName(string name)
        {
            if (name == null)
            {
                return BadRequest(RequriedNameMessage);
            }

            var outputCocktail = await cocktailService.GetByNameAsync(name);

            if (outputCocktail == null)
            {
                return NotFound(string.Format(NotExistingCocktailWithNameMessage, name));
            }

            return Ok(outputCocktail);
        }

        [HttpGet("category/{category}")]
        public async Task<IActionResult> GetByCategoryName(string category)
        {
            if (category == null)
            {
                return BadRequest(RequiredCategoryMessage);
            }

            var cocktails = await cocktailService.GetByCategoryAsync(category);

            if (cocktails == null || !cocktails.Any())
            {
                return NotFound(NotExistingOrEmptyCollectionMessage);
            }

            return Ok(cocktails);
        }

        [HttpPost]
        public async Task<IActionResult> Create(InputCocktailModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(InvalidModelMessage);
            }

            var cocktailServiceModel = this.mapper.Map<InputCocktailServiceModel>(model);
            var outputModel = await cocktailService.CreateAsync(cocktailServiceModel);

            return Ok(outputModel);
        }

        [Authorize]
        [HttpPost("Like/{id}")]
        public async Task<IActionResult> Like(string id)
        {
            var userId = GetUserId();

            if (string.IsNullOrEmpty(userId))
            {
                return BadRequest(UnauthorizedMessage);
            }

            if (id == null)
            {
                return BadRequest(RequiredIdMessage);
            }

            var cocktail = await cocktailService.GetByIdAsync(id);

            if (cocktail == null)
            {
                return NotFound(NotExistingCocktailMessage);
            }

            await cocktailService.UpdateLikes(cocktail, userId);

            return Ok(UpdatedLikesMessage);
        }

        [Authorize]
        [HttpPut("{id:length(24)}")]
        public async Task<IActionResult> Update(string id, InputCocktailModel updatedtCocktail)
        {
            var userId = GetUserId();

            if (string.IsNullOrEmpty(userId))
            {
                return BadRequest(UnauthorizedMessage);
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(InvalidModelMessage);
            }

            var dbCocktail = await cocktailService.GetByIdAsync(id);

            if (dbCocktail == null)
            {
                return NotFound(NotExistingCocktailMessage);
            }

            await cocktailService.UpdateAsync(dbCocktail, updatedtCocktail);

            return Ok(UpdatedCocktailMessage);
        }

        [Authorize]
        [HttpDelete("{id:length(24)}")]
        public async Task<IActionResult> Delete(string id)
        {
            var userId = GetUserId();

            if (string.IsNullOrEmpty(userId))
            {
                return BadRequest(UnauthorizedMessage);
            }

            if (id == null)
            {
                return BadRequest(RequiredIdMessage);
            }

            var cocktail = await cocktailService.GetByIdAsync(id);

            if (cocktail == null)
            {
                return NotFound(NotExistingCocktailMessage);
            }

            await cocktailService.DeleteAsync(id);

            return Ok(DeletedCocktailMessage);
        }
    }
}