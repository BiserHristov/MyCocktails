namespace MyCocktailsApi.Settings
{
    public class CocktailDatabaseSettings : ICocktailDatabaseSettings
    {
        public string CocktailsCollectionName { get; set; }

        public string ConnectionString { get; set; }

        public string DatabaseName { get; set; }
    }
}