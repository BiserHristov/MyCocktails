namespace MyCocktailsApi.Services
{
    using MongoDB.Bson;
    using MongoDB.Bson.Serialization.Attributes;
    using MyCocktailsApi.Models;

    public class InputCocktailServiceModel : InputCocktailModel
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
    }
}
