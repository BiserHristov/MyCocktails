namespace MyCocktailsApi
{
    using MongoDB.Bson.Serialization.Attributes;
    using MyCocktailsApi.InputApiModels;
    using System.Collections.Generic;

    public class RootApiModel
    {
        [BsonElement("drinks")]
        public virtual ICollection<CocktailApiModel> Cocktails { get; init; } = new HashSet<CocktailApiModel>();
    }
}
