namespace MyCocktailsApi.Settings
{
    public interface IUserDatabaseSettings
    {
        string UsersCollectionName { get; set; }

        string ConnectionString { get; set; }

        string DatabaseName { get; set; }
    }
}
