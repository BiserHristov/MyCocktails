namespace MyCocktailsApi
{
    using MongoDB.Bson.Serialization.Attributes;
    using MyCocktailsApi.InputApiModels;
    using Newtonsoft.Json;
    using System.Collections.Generic;

    public class RootApiModel
    {
        [JsonProperty("drinks")]
        public virtual ICollection<CocktailApiModel> Cocktails { get; init; } = new HashSet<CocktailApiModel>();
    }
}
