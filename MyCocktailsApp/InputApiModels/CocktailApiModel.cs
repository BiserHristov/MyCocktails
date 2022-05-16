namespace MyCocktailsApp.InputApiModels
{
    using MongoDB.Bson;
    using MongoDB.Bson.Serialization.Attributes;
    using MyCocktailsApp.Infrastructure;
    using MyCocktailsApp.Data.Models;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Converters;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Runtime.Serialization;
    using System.Threading.Tasks;

    public class CocktailApiModel
    {
        [JsonProperty("strDrink")]
        //[BsonElement("strDrink")]
        public string Name { get; set; }

        [JsonProperty("strCategory")]
        //[BsonElement("strCategory")]
        public string Category { get; set; }

        [JsonProperty("strInstructions")]
        //[BsonElement("strInstructions")]
        public string Instructions { get; set; }

        [JsonProperty("strGlass")]
        // [BsonElement("strGlass")]
        //[BsonRepresentation(BsonType.String)]
        public GlassType GlassType { get; set; }


        [JsonProperty("strDrinkThumb")]
        //[BsonElement("strDrinkThumb")]
        public string PictureUrl { get; set; }

        [JsonProperty("dateModified")]
        //[BsonElement("date")]
        //[BsonDateTimeOptions(Kind = DateTimeKind.Utc)]
        public DateTime DateModified { get; set; }

        //[JsonIgnore]
        [JsonProperty("strIngredient1")]
        public string Ingredient1 { get; set; }

        //[JsonIgnore]
        [JsonProperty("strMeasure1")]
        public string Measure1 { get; set; }

        //[JsonIgnore]
        [JsonProperty("strIngredient2")]
        public string Ingredient2 { get; set; }

        // [JsonIgnore]
        [JsonProperty("strMeasure2")]
        public string Measure2 { get; set; }

        //[JsonIgnore]
        [JsonProperty("strIngredient3")]
        public string Ingredient3 { get; set; }

        //[JsonIgnore]
        [JsonProperty("strMeasure3")]
        public string Measure3 { get; set; }


        //[JsonIgnore]
        [JsonProperty("strIngredient4")]
        public string Ingredient4 { get; set; }

        //[JsonIgnore]
        [JsonProperty("strMeasure4")]
        public string Measure4 { get; set; }

        //[JsonIgnore]
        [JsonProperty("strIngredient5")]
        public string Ingredient5 { get; set; }

        //[JsonIgnore]
        [JsonProperty("strMeasure5")]
        public string Measure5 { get; set; }

        //[JsonIgnore]
        [JsonProperty("strIngredient6")]
        public string Ingredient6 { get; set; }

        //[JsonIgnore]
        [JsonProperty("strMeasure6")]
        public string Measure6 { get; set; }


        //[JsonConverter(typeof(IngredientsConverter))]
        public ICollection<Ingredient> Ingredients { get; set; } = new List<Ingredient>();

        //[BsonElement("date")]
        //[BsonDateTimeOptions(Kind = DateTimeKind.Utc)]
        //public DateTime dateModified { get; set; }

        [OnDeserialized]
        internal void OnDeserialized(StreamingContext context)
        {
            for (int i = 1; i <= 6; i++)
            {
                var ingrName = "Ingredient" + i.ToString();
                var ingrQuantity = "Measure" + i.ToString();
                var name = this.GetType().GetProperty(ingrName).GetValue(this, null);
                var quantity = this.GetType().GetProperty(ingrQuantity).GetValue(this, null);

                if (name == null && quantity == null)
                {
                    continue;
                }

                var ingredient = new Ingredient();
                ingredient.Name = name.ToString();
                ingredient.Quantity = quantity == null ? string.Empty : quantity.ToString();
                Ingredients.Add(ingredient);
            }

        }



    }
}
