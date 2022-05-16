namespace MyCocktailsApp.Controllers
{
    using AutoMapper;
    using Microsoft.AspNetCore.Mvc;
    using MyCocktailsApp.Data.Models;
    using MyCocktailsApp.Models;
    using MyCocktailsApp.Services;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    [Route("api/[controller]")]
    [ApiController]
    public class CocktailsController : ControllerBase
    {
        private readonly ICocktailService cocktailService;
        private readonly IMapper mapper;

        //private IMongoCollection<Post> cocktailCollection;
        public CocktailsController(ICocktailService drinkService, IMapper mapper)
        {
            //var db = client.GetDatabase("myFirstDatabase");
            //cocktailCollection = db.GetCollection<Post>("posts");
            this.cocktailService = drinkService;
            this.mapper = mapper;
            //await SeedData();
        }

        [HttpGet]
        [Route("all", Name = "all")]
        public async Task<ActionResult<IEnumerable<Cocktail>>> GetAll()
        {
            var cocktails = await cocktailService.GetAllAsync();

            if (!cocktails.Any())
            {
                return NotFound();
            }

            return Ok(cocktails);
        }

        [HttpGet]
        [Route("get-by-id/{id}", Name = "getById")]

        public async Task<ActionResult<Cocktail>> GetById(string id)
        {
            var cocktail = await cocktailService.GetByIdAsync(id);

            if (cocktail == null)
            {
                return NotFound();
            }

            return Ok(cocktail);
        }

        [HttpGet]
        [Route("get-by-name/{name}", Name = "getByName")]
        public async Task<ActionResult<Cocktail>> GetByName(string name)
        {
            var cocktail = await cocktailService.GetByNameAsync(name);

            if (cocktail == null)
            {
                return NotFound();
            }

            return Ok(cocktail);
        }

        [HttpGet]
        [Route("get-by-category/{category}", Name = "getByCategory")]
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

            //return CreatedAtActionResult
            return Ok(model);
        }

        [HttpPut("{id:length(24)}")]
        public async Task<IActionResult> Update(string id, Cocktail updatedCocktail)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }


            var cocktail = await cocktailService.GetByIdAsync(id);

            if (cocktail == null)
            {
                return NotFound();
            }

            updatedCocktail.Id = id;

            await cocktailService.UpdateAsync(cocktail, updatedCocktail);

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




        //private async Task SeedData()
        //{
        //    var importedDrinksModel = new CocktailApiModel();
        //    using (var httpClient = new HttpClient())
        //    {
        //        string url = "https://www.thecocktaildb.com/api/json/v2/9973533/randomselection.php";
        //        using (var response = await httpClient.GetAsync(url))
        //        {
        //            string apiResponse = await response.Content.ReadAsStringAsync();
        //            importedDrinksModel = JsonConvert.DeserializeObject<CocktailApiModel>(apiResponse);

        //        }
        //    }
        //    //foreach (var drink in importedDrinksModel.Drinks)
        //    //{
        //    //    await this.Create(drink);
        //    //}
        //    //return importedDrinksModel.Drinks;
        //}

        //[HttpGet]
        //[Route("all")]
        //public IEnumerable<Post> GetAll()
        //{
        //    var allPosts = postService.Get();
        //    //var allPosts = this.cocktailCollection.Find(p => true).ToList();

        //    return allPosts;
        //}

        //[HttpPost]
        //public void Create(Post post)
        //{
        //    postService.Create(post);

        //}
    }
}
