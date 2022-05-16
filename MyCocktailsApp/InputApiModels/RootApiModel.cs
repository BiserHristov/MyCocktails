namespace MyCocktailsApp
{
    using MongoDB.Bson.Serialization.Attributes;
    using MyCocktailsApp.InputApiModels;
    using System.Collections.Generic;

    public class RootApiModel
    {
        [BsonElement("drinks")]
        public virtual ICollection<CocktailApiModel> Drinks { get; init; } = new HashSet<CocktailApiModel>();
    }
}
