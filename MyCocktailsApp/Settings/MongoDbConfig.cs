namespace MyCocktailsApi.Settings
{
    public class MongoDbConfig : IMongoDbConfig
    {
        public string AuthDbName { get; set; }

        public string ConnectionString { get; set; }

    }
}
