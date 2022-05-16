namespace MyCocktailsApp.Data
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;



    public class CocktailDatabaseSettings : ICocktailDatabaseSettings
    {
        public string CocktailsCollectionName { get; set; }
        public string ConnectionString { get; set; }
        public string DatabaseName { get; set; }
    }
}

