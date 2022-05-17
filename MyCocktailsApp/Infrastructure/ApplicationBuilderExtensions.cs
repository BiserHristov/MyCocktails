namespace MyCocktailsApi.Infrastructure
{
    using AutoMapper;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.Extensions.DependencyInjection;
    using MyCocktailsApi.Models;
    using MyCocktailsApi.Services;
    using Newtonsoft.Json;
    using System;
    using System.Linq;
    using System.Net.Http;
    using System.Threading.Tasks;

    public static class ApplicationBuilderExtensions
    {
        public static async Task<IApplicationBuilder> PrepareDatabase(
           this IApplicationBuilder app, IMapper mapper)
        {
            using var scopedServices = app.ApplicationServices.CreateScope();
            var serviceProvider = scopedServices.ServiceProvider;
            await SeedData(serviceProvider, mapper);
            return app;
        }


        private static async Task SeedData(IServiceProvider serviceProvider, IMapper mapper)
        {

            var data = serviceProvider.GetRequiredService<ICocktailService>();
            var dbCocktails = await data.GetAllAsync();

            if (dbCocktails.Any())
            {
                return;
            }

            var importedDrinksModel = new RootApiModel();
            using (var httpClient = new HttpClient())
            {
                string url = "https://www.thecocktaildb.com/api/json/v2/9973533/popular.php";

                using (var response = await httpClient.GetAsync(url))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    importedDrinksModel = JsonConvert.DeserializeObject<RootApiModel>(apiResponse);
                }
            }
            foreach (var apiCocktail in importedDrinksModel.Cocktails)
            {
                var cocktail = mapper.Map<InputCocktailModel>(apiCocktail);
                await data.CreateAsync(cocktail);
            }
        }
    }
}
