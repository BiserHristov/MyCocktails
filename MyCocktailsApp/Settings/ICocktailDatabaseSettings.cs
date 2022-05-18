namespace MyCocktailsApi.Settings
{
    public interface ICocktailDatabaseSettings
    {
        string CocktailsCollectionName { get; set; }

        string ConnectionString { get; set; }

        string DatabaseName { get; set; }
    }
}
