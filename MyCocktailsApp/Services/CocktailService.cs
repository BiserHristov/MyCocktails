namespace MyCocktailsApp.Services
{
    using AutoMapper;
    using MongoDB.Bson;
    using MongoDB.Driver;
    using MyCocktailsApp.Data;
    using MyCocktailsApp.Data.Models;
    using MyCocktailsApp.Models;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    public class CocktailService : ICocktailService
    {
        private readonly IMongoCollection<Cocktail> cocktailsCollection;
        private readonly IMapper mapper;

        //private readonly IConfiguration configuration;

        public CocktailService(ICocktailDatabaseSettings settings, IMapper mapper)
        {
            var client = new MongoClient(settings.ConnectionString);
            var database = client.GetDatabase(settings.DatabaseName);

            cocktailsCollection = database.GetCollection<Cocktail>(settings.CocktailsCollectionName);
            this.mapper = mapper;

            //MongoClient dbClient = new MongoClient(configuration.GetConnectionString("DatabaseConn
            //);
            //postCollection = dbClient.GetDatabase("myFirstDatabase").GetCollection<Post>("posts");
            //var db = mongoClient.GetDatabase(configuration.getdata);
            //postsCollection = db.GetCollection<Post>(configuration.PostsCollectionName);
            //this.configuration = configuration;
        }
        public async Task<List<Cocktail>> GetAllAsync() =>
            await cocktailsCollection.Find(cocktail => true).ToListAsync();


        public async Task<Cocktail> GetByIdAsync(string id) =>
            await cocktailsCollection.Find(cocktail => cocktail.Id == id).FirstOrDefaultAsync();

        public async Task<Cocktail> GetByNameAsync(string name)
        {
            name = name.ToLower();
            return await cocktailsCollection.Find(cocktail => cocktail.Name.ToLower() == name).FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<Cocktail>> GetByCategoryAsync(string category)
        {
            category = category.ToLower();
            return await cocktailsCollection.Find(cocktail => cocktail.Category.ToLower() == category).ToListAsync();

        }
        public async Task<Cocktail> CreateAsync(InputCocktailModel model)
        {
            var cocktail = this.mapper.Map<Cocktail>(model);
            await cocktailsCollection.InsertOneAsync(cocktail);
            return cocktail;
        }

        public async Task UpdateAsync(Cocktail currentCocktail, Cocktail updatedCocktail)
        {
            
            UpdateCocktail(currentCocktail, updatedCocktail);

            await cocktailsCollection.ReplaceOneAsync(c => c.Id == currentCocktail.Id, updatedCocktail);
        }

        //public void Remove(Post postIn) =>
        //    _postCollection.DeleteOne(post => post.Id == postIn.Id);

        public async Task RemoveAsync(string id) =>
            await cocktailsCollection.DeleteOneAsync(cocktail => cocktail.Id == id);



        private void UpdateCocktail(Cocktail dbCocktail, Cocktail updatedCocktail)
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

            var updatedIngredientsCount = updatedCocktail.Ingredients.Count();

            if (updatedIngredientsCount > 0)
            {
                for (int i = 0; i < updatedIngredientsCount; i++)
                {
                    var updatedIngredient = updatedCocktail.Ingredients[i];

                    if (!dbCocktail.Ingredients.Contains(updatedIngredient))
                    {
                        dbCocktail.Ingredients.Add(updatedIngredient);
                    }

                }
            }

        }
    }
}
