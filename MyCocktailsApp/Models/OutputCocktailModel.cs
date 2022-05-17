namespace MyCocktailsApi.Models
{
    using MyCocktailsApi.Data.Models;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    public class OutputCocktailModel
    {
        public string Id { get; set; }
        public string Name { get; set; }

        public string Category { get; set; }

        public string Instructions { get; set; }

        public int Likes { get; set; }

        public string Glass { get; set; }

        public string? PictureUrl { get; set; }

        public IList<OutputIngredientModel> Ingredients { get; init; } = new List<OutputIngredientModel>();

        public DateTime DateModified { get; set; }


    }
}
