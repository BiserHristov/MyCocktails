namespace MyCocktailsApi.Services
{
    using AutoMapper;
    using MongoDB.Driver;
    using MongoDB.Driver.Linq;
    using MyCocktailsApi.Settings;
    using MyCocktailsApi.Data.Models;
    using MyCocktailsApi.Models;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.Extensions.Logging;
    using System;

    public class CocktailService : ICocktailService
    {
        private readonly IMongoCollection<Cocktail> cocktailsCollection;
        private readonly IMapper mapper;
        private readonly ILogger<CocktailService> logger;

        public CocktailService(ICocktailDatabaseSettings settings, IMapper mapper, ILogger<CocktailService> logger)
        {
            var client = new MongoClient(settings.ConnectionString);
            var database = client.GetDatabase(settings.DatabaseName);

            cocktailsCollection = database.GetCollection<Cocktail>(settings.CocktailsCollectionName);
            this.mapper = mapper;
            this.logger = logger;
        }
        public async Task<IEnumerable<OutputCocktailModel>> GetAllAsync()
        {
            var dbCocktails = await cocktailsCollection.Find(cocktail => true).ToListAsync();
            return this.mapper.Map<IEnumerable<OutputCocktailModel>>(dbCocktails);
        }

        public async Task<OutputCocktailModel> GetByIdAsync(string id)
        {
            var cocktailModel = await cocktailsCollection.Find(cocktail => cocktail.Id == id).FirstOrDefaultAsync();
            return this.mapper.Map<OutputCocktailModel>(cocktailModel);
        }

        public async Task<OutputCocktailModel> GetByNameAsync(string name)
        {
            name = name.ToLower();
            var cocktailModel = await cocktailsCollection.Find(cocktail => cocktail.Name.ToLower() == name).FirstOrDefaultAsync();
            return this.mapper.Map<OutputCocktailModel>(cocktailModel);

        }

        public async Task<IEnumerable<OutputCocktailModel>> GetByCategoryAsync(string category)
        {
            category = category.ToLower();
            var cocktailModel = await cocktailsCollection.Find(cocktail => cocktail.Category.ToLower() == category).ToListAsync();
            return this.mapper.Map<IEnumerable<OutputCocktailModel>>(cocktailModel);
        }

        public async Task<Cocktail> CreateAsync(InputCocktailModel model)
        {
            Cocktail cocktail = new Cocktail();

            try
            {
                cocktail = this.mapper.Map<Cocktail>(model);
                await cocktailsCollection.InsertOneAsync(cocktail);

            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Failed to add new cocktail!");
            }

            return cocktail;
        }

        public async Task UpdateAsync(UpdateCocktailModel currentCocktail, InputCocktailModel updatedCocktail)
        {
            try
            {
                UpdateCocktail(currentCocktail, updatedCocktail);

                await cocktailsCollection.ReplaceOneAsync(c => c.Id == currentCocktail.Id, this.mapper.Map<Cocktail>(updatedCocktail));
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Failed to update the cocktail!");
            }
        }

        public async Task RemoveAsync(string id) =>
            await cocktailsCollection.DeleteOneAsync(cocktail => cocktail.Id == id);

        private void UpdateCocktail(UpdateCocktailModel dbCocktail, InputCocktailModel updatedCocktail)
        {
            if (!string.IsNullOrWhiteSpace(updatedCocktail.Name))
            {
                dbCocktail.Name = updatedCocktail.Name;
            }

            if (updatedCocktail.Likes >= 0)
            {
                dbCocktail.Likes = updatedCocktail.Likes;
            }
            if (!string.IsNullOrWhiteSpace(updatedCocktail.Category))
            {
                dbCocktail.Category = updatedCocktail.Category;
            }

            dbCocktail.Ingredients.Clear();

            var updatedIngredientsCount = updatedCocktail.Ingredients.Count();

            if (updatedIngredientsCount > 0)
            {
                for (int i = 0; i < updatedIngredientsCount; i++)
                {
                    dbCocktail.Ingredients.Add(updatedCocktail.Ingredients[i]);

                }
            }
        }
    }
}
