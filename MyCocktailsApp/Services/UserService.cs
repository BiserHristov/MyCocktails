namespace MyCocktailsApi.Services
{
    using System.Threading.Tasks;
    using Microsoft.Extensions.Logging;
    using MongoDB.Driver;
    using MyCocktailsApi.Data.Models;
    using MyCocktailsApi.Settings;

    public class UserService : IUserService
    {
        private readonly IMongoCollection<User> userCollection;
        private readonly ILogger<UserService> logger;

        public UserService(IUserDatabaseSettings settings, ILogger<UserService> logger)
        {
            var client = new MongoClient(settings.ConnectionString);
            var database = client.GetDatabase(settings.DatabaseName);
            userCollection = database.GetCollection<User>(settings.UsersCollectionName);
            this.logger = logger;
        }

        public async Task<bool> UserExist(string email)
        {
            var user = await userCollection.Find(u => u.Email == email).FirstOrDefaultAsync();

            if (user == null)
            {
                return false;
            }

            return true;
        }
    }
}
