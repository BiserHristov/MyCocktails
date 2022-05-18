namespace MyCocktailsApi.Controllers
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using AutoMapper;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using MyCocktailsApi.Data.Models;
    using MyCocktailsApi.Infrastructure;
    using MyCocktailsApi.Models;
    using MyCocktailsApi.Services;

    [Route("api/[controller]")]
    [ApiController]
    public class CocktailsController : ControllerBase
    {
        private readonly ICocktailService cocktailService;

        public CocktailsController(ICocktailService cocktailService)
        {
            this.cocktailService = cocktailService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var outputCocktails = await cocktailService.GetAllAsync();

            if (!outputCocktails.Any())
            {
                return NotFound();
            }

            return Ok(outputCocktails);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(string id)
        {
            var outputCocktail = await cocktailService.GetByIdAsync(id);

            if (outputCocktail == null)
            {
                return NotFound();
            }

            return Ok(outputCocktail);
        }

        [HttpGet("name/{name}")]
        public async Task<IActionResult> GetByName(string name)
        {
            var outputCocktail = await cocktailService.GetByNameAsync(name);

            if (outputCocktail == null)
            {
                return NotFound();
            }

            return Ok(outputCocktail);
        }

        [HttpGet("category/{category}")]
        public async Task<ActionResult<IEnumerable<Cocktail>>> GetByCategoryName(string category)
        {
            var cocktails = await cocktailService.GetByCategoryAsync(category);

            if (!cocktails.Any())
            {
                return NotFound();
            }

            return Ok(cocktails);
        }

        [HttpPost]
        public async Task<ActionResult<Cocktail>> Create(InputCocktailModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var insertedDbModel = await cocktailService.CreateAsync(model);

            return Ok(insertedDbModel);
        }

        [Authorize]
        [HttpPost("Like/{id}")]
        public async Task<IActionResult> Like(string id)
        {
            var cocktail = await cocktailService.GetByIdAsync(id);

            if (cocktail == null)
            {
                return NotFound();
            }

            var userId = this.User.GetId();

            await cocktailService.UpdateLikes(cocktail, userId);

            return Ok("Likes were updated");
        }

        [HttpPut("{id:length(24)}")]
        public async Task<IActionResult> Update(string id, InputCocktailModel updatedtCocktail)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var dbCocktail = await cocktailService.GetByIdAsync(id);

            if (dbCocktail == null)
            {
                return NotFound();
            }

            await cocktailService.UpdateAsync(dbCocktail, updatedtCocktail);

            return Ok("The cocktail is updated.");
        }

        [HttpDelete("{id:length(24)}")]
        public async Task<IActionResult> Delete(string id)
        {
            var cocktail = await cocktailService.GetByIdAsync(id);

            if (cocktail == null)
            {
                return NotFound();
            }

            await cocktailService.RemoveAsync(id);

            return Ok("The cocktail is deleted.");
        }
    }
}