namespace MyCocktailsApi.Controllers
{
    using System;
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

    using static ApiConstants.Cocktail;

    [Route("api/[controller]")]
    [ApiController]
    public class CocktailsController : ControllerBase
    {
        private readonly ICocktailService cocktailService;
        private readonly IMapper mapper;

        public CocktailsController(ICocktailService cocktailService, IMapper mapper)
        {
            this.cocktailService = cocktailService;
            this.mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var outputCocktails = await cocktailService.GetAllAsync();

            if (outputCocktails == null || !outputCocktails.Any())
            {
                return NotFound();
            }

            return Ok(outputCocktails);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

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
            if (name == null)
            {
                return NotFound();
            }

            var outputCocktail = await cocktailService.GetByNameAsync(name);

            if (outputCocktail == null)
            {
                return NotFound();
            }

            return Ok(outputCocktail);
        }

        [HttpGet("category/{category}")]
        public async Task<IActionResult> GetByCategoryName(string category)
        {
            if (category == null)
            {
                return NotFound();
            }

            var cocktails = await cocktailService.GetByCategoryAsync(category);

            if (cocktails == null || !cocktails.Any())
            {
                return NotFound();
            }

            return Ok(cocktails);
        }

        [HttpPost]
        public async Task<IActionResult> Create(InputCocktailModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var cocktailServiceModel = this.mapper.Map<InputCocktailServiceModel>(model);
            var outputModel = await cocktailService.CreateAsync(cocktailServiceModel);

            return Ok(outputModel);
        }

        [Authorize]
        [HttpPost("Like/{id}")]
        public async Task<IActionResult> Like(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var cocktail = await cocktailService.GetByIdAsync(id);

            if (cocktail == null)
            {
                return NotFound();
            }

            var userId = this.User.GetId();

            await cocktailService.UpdateLikes(cocktail, userId);

            return Ok(UpdatedLikesMessage);
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

            return Ok(UpdatedCocktailMessage);
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

            return Ok(DeletedCocktailMessage);
        }
    }
}