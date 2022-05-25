namespace MyCocktailsApi.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Security.Claims;
    using System.Threading.Tasks;
    using AutoMapper;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Logging;
    using MyCocktailsApi.Models;
    using MyCocktailsApi.Services;
    using static ApiConstants.Cocktail;

    [Route("api/[controller]")]
    [ApiController]
    public class CocktailsController : ControllerBase
    {
        public Func<string> GetUserId;
        private readonly ICocktailService cocktailService;
        private readonly IMapper mapper;

        public CocktailsController(ICocktailService cocktailService, IMapper mapper)
        {
            this.cocktailService = cocktailService;
            this.mapper = mapper;
            GetUserId = () => User.FindFirst(ClaimTypes.Email).Value;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var resultCocktails = await cocktailService.GetAllAsync();

            if (resultCocktails == null || !resultCocktails.Any())
            {
                return NotFound(NotExistingOrEmptyCollectionMessage);
            }

            var outputCocktails = this.mapper.Map<IEnumerable<OutputCocktailModel>>(resultCocktails);
            return Ok(outputCocktails);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(string id)
        {
            try
            {
                if (id == null)
                {
                    return BadRequest(RequiredIdMessage);
                }

                var searchedCocktail = await cocktailService.GetByIdAsync(id);

                if (searchedCocktail == null)
                {
                    return NotFound(NotExistingCocktailMessage);
                }

                var outputCocktail = this.mapper.Map<OutputCocktailModel>(searchedCocktail);
                return Ok(outputCocktail);
            }
            catch (Exception)
            {
                return BadRequest("Failed to get the cocktail by id! Check the \"id\" and try again.");
            }

        }

        [HttpGet("name/{name}")]
        public async Task<IActionResult> GetByName(string name)
        {
            try
            {
                if (name == null)
                {
                    return BadRequest(RequriedNameMessage);
                }

                var searchedCocktail = await cocktailService.GetByNameAsync(name);

                if (searchedCocktail == null)
                {
                    return NotFound(string.Format(NotExistingCocktailWithNameMessage, name));
                }

                var outputCocktail = this.mapper.Map<OutputCocktailModel>(searchedCocktail);

                return Ok(outputCocktail);
            }
            catch (Exception)
            {
                return BadRequest("Failed to get the cocktail by name! Check the \"name\" and try again.");
            }
        }

        [HttpGet("category/{category}")]
        public async Task<IActionResult> GetByCategoryName(string category)
        {
            try
            {
                if (category == null)
                {
                    return BadRequest(RequiredCategoryMessage);
                }

                var resultCocktails = await cocktailService.GetByCategoryAsync(category);

                if (resultCocktails == null || !resultCocktails.Any())
                {
                    return NotFound(NotExistingOrEmptyCollectionMessage);
                }

                var outputCocktails = this.mapper.Map<IEnumerable<OutputCocktailModel>>(resultCocktails);

                return Ok(outputCocktails);
            }
            catch (Exception)
            {
                return BadRequest("Failed to get the cocktail by \"category\"! Check the \"category\" and try again.");
            }
        }

        [HttpPost]
        public async Task<IActionResult> Create(InputCocktailModel model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(InvalidModelMessage);
                }

                var cocktailServiceModel = this.mapper.Map<CocktailServiceModel>(model);
                var insertedModel = await cocktailService.CreateAsync(cocktailServiceModel);
                var outputCocktail = this.mapper.Map<OutputCocktailModel>(insertedModel);
                return Ok(outputCocktail);
            }
            catch (Exception)
            {
                return BadRequest("Failed to create the cocktail! Check the cocktail and try again.");

            }
        }

        [Authorize]
        [HttpPost("Like/{cocktailId}")]
        public async Task<IActionResult> Like(string cocktailId)
        {
            try
            {
                var userId = GetUserId();

                if (string.IsNullOrEmpty(userId))
                {
                    return BadRequest(UnauthorizedMessage);
                }

                if (cocktailId == null)
                {
                    return BadRequest(RequiredIdMessage);
                }

                var searchedCocktail = await cocktailService.GetByIdAsync(cocktailId);

                if (searchedCocktail == null)
                {
                    return NotFound(NotExistingCocktailMessage);
                }

                await cocktailService.UpdateLikes(searchedCocktail, userId);

                return Ok(UpdatedLikesMessage);
            }
            catch (Exception)
            {
                return BadRequest("Failed to like the cocktail. Please try again!");
            }
        }

        [Authorize]
        [HttpPut("{cocktailId:length(24)}")]
        public async Task<IActionResult> Update(string cocktailId, InputCocktailModel updatedtCocktail)
        {
            try
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

                var searchedCocktail = await cocktailService.GetByIdAsync(cocktailId);

                if (searchedCocktail == null)
                {
                    return NotFound(NotExistingCocktailMessage);
                }

                var updatedCocktailServiceModel = this.mapper.Map<CocktailServiceModel>(updatedtCocktail);

                await cocktailService.UpdateAsync(searchedCocktail, updatedCocktailServiceModel);

                return Ok(UpdatedCocktailMessage);
            }
            catch (Exception)
            {
                return BadRequest("Failed to update the cocktail! Please try again.");
            }
        }

        [Authorize]
        [HttpDelete("{cocktailId:length(24)}")]
        public async Task<IActionResult> Delete(string cocktailId)
        {
            try
            {
                var userId = GetUserId();

                if (string.IsNullOrEmpty(userId))
                {
                    return BadRequest(UnauthorizedMessage);
                }

                if (cocktailId == null)
                {
                    return BadRequest(RequiredIdMessage);
                }

                var searchedCocktail = await cocktailService.GetByIdAsync(cocktailId);

                if (searchedCocktail == null)
                {
                    return NotFound(NotExistingCocktailMessage);
                }

                await cocktailService.DeleteAsync(cocktailId);

                return Ok(DeletedCocktailMessage);
            }
            catch (Exception)
            {
                return BadRequest("Failed to delete the cocktail! Please try again.");
            }
        }
    }
}