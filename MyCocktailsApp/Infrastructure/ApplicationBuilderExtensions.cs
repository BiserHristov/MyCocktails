namespace MyCocktailsApp.Infrastructure
{
    using Microsoft.AspNetCore.Builder;
    using Microsoft.Extensions.DependencyInjection;
    using MyCocktailsApp.Data.Models;
    using MyCocktailsApp.InputApiModels;
    using MyCocktailsApp.Models;
    using MyCocktailsApp.Services;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Serialization;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net.Http;
    using System.Threading.Tasks;

    public static class ApplicationBuilderExtensions
    {
        public static async Task<IApplicationBuilder> PrepareDatabase(
           this IApplicationBuilder app)
        {
            using var scopedServices = app.ApplicationServices.CreateScope();
            var serviceProvider = scopedServices.ServiceProvider;
            await SeedData(serviceProvider);
            return app;
        }


        private static async Task SeedData(IServiceProvider serviceProvider)
        {
            var data = serviceProvider.GetRequiredService<ICocktailService>();
            var dbDrinks = await data.GetAllAsync();

            if (dbDrinks.Any())
            {
                return;

            }

            var importedDrinksModel = new RootApiModel();
            using (var httpClient = new HttpClient())
            {
                //string url = "https://www.thecocktaildb.com/api/json/v2/9973533/randomselection.php";
                string url = "https://www.thecocktaildb.com/api/json/v2/9973533/popular.php";

                using (var response = await httpClient.GetAsync(url))
                {

                    string apiResponse = await response.Content.ReadAsStringAsync();
                    importedDrinksModel = JsonConvert.DeserializeObject<RootApiModel>(apiResponse);

                }
            }
            foreach (var drink in importedDrinksModel.Drinks)
            {
                var cocktail = new InputCocktailModel();
                cocktail.Name = drink.Name;
                cocktail.Category = drink.Category;
                cocktail.Instructions = drink.Instructions;
                cocktail.Likes = 0;
                cocktail.Glass = drink.GlassType.ToString();
                cocktail.DateModified = drink.DateModified;
                cocktail.PictureUrl = drink.PictureUrl;
                foreach (var ingredient in drink.Ingredients)
                {
                    cocktail.Ingredients.Add(ingredient);
                }

                await data.CreateAsync(cocktail);
            }
        }
    }
}
