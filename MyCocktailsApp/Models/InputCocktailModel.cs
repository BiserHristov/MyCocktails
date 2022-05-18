﻿namespace MyCocktailsApi.Models
{
    using MongoDB.Bson.Serialization.Attributes;
    using Newtonsoft.Json;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Runtime.Serialization;
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
        public string? PictureUrl { get; set; }

        [Required]
        public IList<InputIngredientModel> Ingredients { get; init; } = new List<InputIngredientModel>();

        [Required]
        [DataType(DataType.DateTime)]
        public DateTime DateModified { get; set; }

    }
}
