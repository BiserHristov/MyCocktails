namespace MyCocktailsApi.InputApiModels
{
    using System;
    using System.Collections.Generic;
    using System.Runtime.Serialization;
    using MyCocktailsApi.Data;
    using MyCocktailsApi.Models;
    using Newtonsoft.Json;

    public class CocktailApiModel
    {
        [JsonProperty("strDrink")]
        public string Name { get; set; }

        [JsonProperty("strCategory")]
        public string Category { get; set; }

        [JsonProperty("strInstructions")]
        public string Instructions { get; set; }

        [JsonProperty("strGlass")]
        public GlassType GlassType { get; set; }

        [JsonProperty("strDrinkThumb")]
        public string PictureUrl { get; set; }

        [JsonProperty("dateModified")]
        public DateTime DateModified { get; set; }

        [JsonProperty("strIngredient1")]
        public string Ingredient1 { get; set; }

        [JsonProperty("strMeasure1")]
        public string Measure1 { get; set; }

        [JsonProperty("strIngredient2")]
        public string Ingredient2 { get; set; }

        [JsonProperty("strMeasure2")]
        public string Measure2 { get; set; }

        [JsonProperty("strIngredient3")]
        public string Ingredient3 { get; set; }

        [JsonProperty("strMeasure3")]
        public string Measure3 { get; set; }

        [JsonProperty("strIngredient4")]
        public string Ingredient4 { get; set; }

        [JsonProperty("strMeasure4")]
        public string Measure4 { get; set; }

        [JsonProperty("strIngredient5")]
        public string Ingredient5 { get; set; }

        [JsonProperty("strMeasure5")]
        public string Measure5 { get; set; }

        [JsonProperty("strIngredient6")]
        public string Ingredient6 { get; set; }

        [JsonProperty("strMeasure6")]
        public string Measure6 { get; set; }

        public ICollection<InputIngredientModel> Ingredients { get; set; } = new List<InputIngredientModel>();

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

                var ingredient = new InputIngredientModel();
                ingredient.Name = name.ToString();
                ingredient.Quantity = quantity == null ? string.Empty : quantity.ToString();
                Ingredients.Add(ingredient);
            }
        }
    }
}