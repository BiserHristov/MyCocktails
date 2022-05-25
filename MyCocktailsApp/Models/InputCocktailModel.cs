namespace MyCocktailsApi.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using MyCocktailsApi.Infrastructure;
    using Newtonsoft.Json;
    using static MyCocktailsApi.Data.DataConstants.Cocktail;

    public class InputCocktailModel
    {
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
        public string Glass { get; set; }

        [Url]
        public string PictureUrl { get; set; }

        [Required]
        public IList<InputIngredientModel> Ingredients { get; set; } = new List<InputIngredientModel>();

        [Required]
        [CheckDateRange]
        [DataType(DataType.DateTime)]
        public DateTime DateModified { get; set; }
    }
}
