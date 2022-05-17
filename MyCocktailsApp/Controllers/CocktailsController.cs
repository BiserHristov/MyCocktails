namespace MyCocktailsApi.Controllers
{
    using AutoMapper;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Logging;
    using MyCocktailsApi.Data.Models;
    using MyCocktailsApi.Infrastructure;
    using MyCocktailsApi.Models;
    using MyCocktailsApi.Services;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    [Route("api/[controller]")]
    [ApiController]
    public class CocktailsController : ControllerBase
    {
        private readonly ICocktailService cocktailService;
        private readonly IMapper mapper;

        public CocktailsController(ICocktailService drinkService, IMapper mapper)
        {
            this.cocktailService = drinkService;
            this.mapper = mapper;
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
            var cocktail = await cocktailService.GetByCategoryAsync(category);

            if (cocktail == null)
            {
                return NotFound();
            }

            return Ok(cocktail);
        }

        [HttpPost]
        public async Task<ActionResult<Cocktail>> Create(InputCocktailModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            await cocktailService.CreateAsync(model);

            return Ok(model);
        }

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

            var updateCocktailModel = this.mapper.Map<UpdateCocktailModel>(dbCocktail);
            await cocktailService.UpdateAsync(updateCocktailModel, updatedtCocktail);

            return NoContent();
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

            return NoContent();
        }

    }
}
