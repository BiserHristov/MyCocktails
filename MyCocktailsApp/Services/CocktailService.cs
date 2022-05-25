namespace MyCocktailsApi.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using AutoMapper;
    using Microsoft.Extensions.Logging;
    using MongoDB.Driver;
    using MongoDB.Driver.Linq;
    using MyCocktailsApi.Data.Models;
    using MyCocktailsApi.Models;
    using MyCocktailsApi.Settings;
    using static ApiConstants.CocktailService;

    public class CocktailService : ICocktailService
    {
        private readonly IMongoCollection<Cocktail> cocktailCollection;
        private readonly IMapper mapper;
        private readonly ILogger<CocktailService> logger;

        public CocktailService(ICocktailDatabaseSettings settings, IMapper mapper, ILogger<CocktailService> logger)
        {
            var client = new MongoClient(settings.ConnectionString);
            var database = client.GetDatabase(settings.DatabaseName);

            cocktailCollection = database.GetCollection<Cocktail>(settings.CocktailsCollectionName);
            this.mapper = mapper;
            this.logger = logger;
        }

        public async Task<IEnumerable<CocktailServiceModel>> GetAllAsync()
        {
            var dbCocktails = await cocktailCollection.Find(cocktail => true).ToListAsync();
            var mappedCocktails = mapper.Map<IEnumerable<CocktailServiceModel>>(dbCocktails);
            return mappedCocktails;
        }

        public async Task<CocktailServiceModel> GetByIdAsync(string id)
        {
            var cocktailModel = new CocktailServiceModel();

            try
            {
                var dbCocktail = await cocktailCollection.Find(cocktail => cocktail.Id == id).FirstOrDefaultAsync();
                cocktailModel = mapper.Map<CocktailServiceModel>(dbCocktail);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, FailedByIdMessage);
                throw new ArgumentNullException();
            }

            return cocktailModel;
        }

        public async Task<CocktailServiceModel> GetByNameAsync(string name)
        {
            var cocktailModel = new CocktailServiceModel();

            try
            {
                name = name.ToLower();
                var dbCocktail = await cocktailCollection.Find(cocktail => cocktail.Name.ToLower() == name).FirstOrDefaultAsync();
                cocktailModel = mapper.Map<CocktailServiceModel>(dbCocktail);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, FailedByNameMessage);
                throw new ArgumentNullException();
            }

            return cocktailModel;
        }

        public async Task<IEnumerable<CocktailServiceModel>> GetByCategoryAsync(string category)
        {
            try
            {
                category = category.ToLower();
                var cocktailModel = await cocktailCollection.Find(cocktail => cocktail.Category.ToLower() == category).ToListAsync();
                var mappedReesult = mapper.Map<IEnumerable<CocktailServiceModel>>(cocktailModel);
                return mappedReesult;
            }
            catch (Exception ex)
            {
                logger.LogError(ex, FailedByCategoryMessage);
                throw new ArgumentNullException();
            }
        }

        public async Task<CocktailServiceModel> CreateAsync(CocktailServiceModel model)
        {
            try
            {
                var dbCocktail = mapper.Map<Cocktail>(model);
                await cocktailCollection.InsertOneAsync(dbCocktail);
                var cocktail = mapper.Map<CocktailServiceModel>(dbCocktail);
                return cocktail;

            }
            catch (Exception ex)
            {
                logger.LogError(ex, FailedCreateMessage);
                throw new ArgumentNullException();
            }

        }

        public async Task UpdateAsync(CocktailServiceModel dbCocktail, CocktailServiceModel updatedCocktail)
        {
            try
            {
                UpdateCocktail(dbCocktail, updatedCocktail);

                var cocktail = mapper.Map<Cocktail>(dbCocktail);

                await cocktailCollection.ReplaceOneAsync(c => c.Id == dbCocktail.Id, cocktail);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, FailedUpdateCocktailMessage);
                throw new ArgumentNullException();
            }
        }

        public async Task UpdateLikes(CocktailServiceModel model, string userId)
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

                var dbCocktail = mapper.Map<Cocktail>(model);
                await cocktailCollection.ReplaceOneAsync(c => c.Id == dbCocktail.Id, dbCocktail);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, FailedUpdateLikesMessage);
                throw new ArgumentNullException();
            }
        }

        public async Task DeleteAsync(string id)
        {
            try
            {
                await cocktailCollection.DeleteOneAsync(cocktail => cocktail.Id == id);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, FailedUDeleteCocktailMessage);
                throw new ArgumentNullException();
            }
        }
            

        private void UpdateCocktail(CocktailServiceModel dbCocktail, CocktailServiceModel updatedCocktail)
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

            if (!string.IsNullOrWhiteSpace(updatedCocktail.PictureUrl) &&
               updatedCocktail.Instructions != dbCocktail.PictureUrl)
            {
                dbCocktail.PictureUrl = updatedCocktail.PictureUrl;
            }

            if (dbCocktail.Glass != updatedCocktail.Glass)
            {
                dbCocktail.Glass = updatedCocktail.Glass;
            }

            dbCocktail.DateModified = DateTime.UtcNow;

            dbCocktail.Ingredients.Clear();

            var updatedIngredientsCount = updatedCocktail.Ingredients.Count();

            if (updatedIngredientsCount > 0)
            {
                for (int i = 0; i < updatedIngredientsCount; i++)
                {
                    var dbCocktailIngredent = mapper.Map<InputIngredientModel>(updatedCocktail.Ingredients[i]);
                    dbCocktail.Ingredients.Add(dbCocktailIngredent);
                }
            }
        }
    }
}
