namespace MyCocktailsApi.Settings
{
    interface IMongoDbConfig
    {
        public string AuthDbName { get; set; }

        public string ConnectionString { get; set; }
    }
}
