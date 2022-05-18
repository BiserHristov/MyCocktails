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
    using MyCocktailsApi.InputApiModels;

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

        public async Task<OutputCocktailModel> CreateAsync(InputCocktailModel model)
        {
            OutputCocktailModel cocktail = new OutputCocktailModel();

            try
            {
                var dbCocktail = this.mapper.Map<Cocktail>(model);
                await cocktailsCollection.InsertOneAsync(dbCocktail);
                cocktail = this.mapper.Map<OutputCocktailModel>(dbCocktail);

            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Failed to add new cocktail!");
            }

            return cocktail;
        }

        public async Task UpdateAsync(OutputCocktailModel dbCocktail, InputCocktailModel updatedCocktail)
        {
            try
            {
                UpdateCocktail(dbCocktail, updatedCocktail);

                var cocktail = this.mapper.Map<Cocktail>(dbCocktail);

                await cocktailsCollection.ReplaceOneAsync(c => c.Id == dbCocktail.Id, cocktail);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Failed to update the cocktail!");
            }
        }

        public async Task UpdateLikes(OutputCocktailModel model, string userId)
        {
            try
            {
                if (model.UsersLike.Contains(userId))
                {
                    model.UsersLike.Remove(userId);
                }
                else
                {
                    model.UsersLike.Add(userId);
                }

                var dbCocktail = this.mapper.Map<Cocktail>(model);
                await cocktailsCollection.ReplaceOneAsync(c => c.Id == dbCocktail.Id, dbCocktail);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Failed to update likes!");
            }
        }

        public async Task RemoveAsync(string id) =>
            await cocktailsCollection.DeleteOneAsync(cocktail => cocktail.Id == id);

        private void UpdateCocktail(OutputCocktailModel dbCocktail, InputCocktailModel updatedCocktail)
        {
            if (!string.IsNullOrWhiteSpace(updatedCocktail.Name) &&
                updatedCocktail.Name != dbCocktail.Name)
            {
                dbCocktail.Name = updatedCocktail.Name;
            }

            if (!string.IsNullOrWhiteSpace(updatedCocktail.Category) &&
               updatedCocktail.Category != dbCocktail.Category)
            {
                dbCocktail.Category = updatedCocktail.Category;
            }

            if (!string.IsNullOrWhiteSpace(updatedCocktail.Instructions) &&
               updatedCocktail.Instructions != dbCocktail.Instructions)
            {
                dbCocktail.Instructions = updatedCocktail.Instructions;
            }

            if (dbCocktail.Glass != updatedCocktail.Glass)
            {
                dbCocktail.Glass = updatedCocktail.Glass;
            }

            if (dbCocktail.DateModified != updatedCocktail.DateModified)
            {
                dbCocktail.DateModified = updatedCocktail.DateModified;
            }

            dbCocktail.Ingredients.Clear();

            var updatedIngredientsCount = updatedCocktail.Ingredients.Count();

            if (updatedIngredientsCount > 0)
            {
                for (int i = 0; i < updatedIngredientsCount; i++)
                {
                    var dbCocktailIngredent = this.mapper.Map<OutputIngredientModel>(updatedCocktail.Ingredients[i]);
                    dbCocktail.Ingredients.Add(dbCocktailIngredent);

                }
            }
        }

        public Task<OutputCocktailModel> Like(string id)
        {
            throw new NotImplementedException();
        }
    }
}
