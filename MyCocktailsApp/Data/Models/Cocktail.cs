namespace MyCocktailsApp.Data.Models
{
    using MongoDB.Bson;
    using MongoDB.Bson.Serialization.Attributes;
    using MyCocktailsApp.Infrastructure;
    using MyCocktailsApp.InputApiModels;
    using Newtonsoft.Json;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using static MyCocktailsApp.Data.DataConstants.Cocktail;

    [BsonIgnoreExtraElements]
    public class Cocktail
    {
        [BsonId]
        //[BsonIgnoreIfDefault]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        //[BsonElement("strDrink")]
        [Required]
        [StringLength(NameMaxLength, MinimumLength = NameMinLength)]
        public string Name { get; set; }

        //[BsonElement("strCategory")]
        [Required]
        [StringLength(CategoryMaxLength, MinimumLength = CategoryMinLength)]
        public string Category { get; set; }

        //[JsonProperty("strInstructions")]
        [Required]
        [StringLength(InstructionsMaxLength, MinimumLength = InstructionsMinLength)]
        public string Instructions { get; set; }

        //[BsonElement("likes")]
        [Required]
        [Range(0, int.MaxValue)]
        public int Likes { get; set; }

        [Required]
        [EnumDataType(typeof(GlassType))]
        [BsonRepresentation(BsonType.String)]
        [JsonProperty("Glass type")]
        //[BsonRepresentation(BsonType.String)]
        public GlassType Glass { get; set; }

        [Required]
        [Url]
        [BsonElement("Pucture URL")]
        [JsonProperty("Pucture URL")]
        public string PictureUrl { get; set; }

        [Required]
        [NotEmptyCollection]
        public IList<Ingredient> Ingredients { get; init; } = new List<Ingredient>();

        //[BsonElement("date")]
        //[BsonDateTimeOptions(Kind = DateTimeKind.Utc)]
        [Required]
        [DataType(DataType.DateTime)]
        [BsonElement("Date Modified")]
        [BsonDateTimeOptions(Kind = DateTimeKind.Utc)]
        [JsonProperty("Date Modified")]
        public DateTime DateModified { get; set; }


    }
}
//title: 'Post One',
//    body: 'Body of post one',
//    category: 'News',
//    likes: 10,
//    tags:[ 'news', 'events' ],
//    date: 'Wed May 11 2022 15:18:53 GMT+0300 (Източноевропейско лятно часово време)'