namespace MyCocktailsApi.Services
{
    using System.Collections.Generic;
    using System.Runtime.Serialization;
    using MongoDB.Bson;
    using MongoDB.Bson.Serialization.Attributes;
    using MyCocktailsApi.Models;

    public class CocktailServiceModel : InputCocktailModel
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]

        public string Id { get; set; }

        public int Likes { get; set; }

        [IgnoreDataMember]
        public IList<string> UsersLike { get; set; } = new List<string>();
    }
}
