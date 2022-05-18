namespace MyCocktailsApi.Data.Models
{
    using MongoDB.Bson;
    using MongoDB.Bson.Serialization.Attributes;
    using MyCocktailsApi.Infrastructure;
    using MyCocktailsApi.InputApiModels;
    using Newtonsoft.Json;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Runtime.Serialization;
    using static MyCocktailsApi.Data.DataConstants.Cocktail;

    [BsonIgnoreExtraElements]
    public class Cocktail
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        [Required]
        [StringLength(NameMaxLength, MinimumLength = NameMinLength)]
        public string Name { get; set; }

        [Required]
        [StringLength(CategoryMaxLength, MinimumLength = CategoryMinLength)]
        public string Category { get; set; }

        [Required]
        [StringLength(InstructionsMaxLength, MinimumLength = InstructionsMinLength)]
        public string Instructions { get; set; }

        [Required]
        [Range(0, int.MaxValue)]
        public int Likes
        {
            get
            {
                return UsersLike.Count;
            }
        }

        [IgnoreDataMember]
        public IList<string> UsersLike { get; set; } = new List<string>();

        [Required]
        [EnumDataType(typeof(GlassType))]
        [BsonRepresentation(BsonType.String)]
        [JsonProperty("Glass type")]
        public GlassType Glass { get; set; }

        [Required]
        [Url]
        [BsonElement("Pucture URL")]
        [JsonProperty("Pucture URL")]
        public string PictureUrl { get; set; }

        [Required]
        [NotEmptyCollection]
        public IList<Ingredient> Ingredients { get; init; } = new List<Ingredient>();

        [Required]
        [DataType(DataType.DateTime)]
        [BsonElement("Date Modified")]
        [BsonDateTimeOptions(Kind = DateTimeKind.Utc)]
        [JsonProperty("Date Modified")]
        public DateTime DateModified { get; set; }
    }
}