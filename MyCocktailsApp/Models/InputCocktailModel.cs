namespace MyCocktailsApp.Models
{
    using MyCocktailsApp.Data.Models;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using System.Threading.Tasks;

    using static MyCocktailsApp.Data.DataConstants.Cocktail;

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
        [Range(0, int.MaxValue)]
        public int Likes { get; set; }

        [Required]
        public string Glass { get; set; }


        [Url]
        public string? PictureUrl { get; set; }

        [Required]
        public IList<Ingredient> Ingredients { get; init; } = new List<Ingredient>();

        //[BsonElement("date")]
        //[BsonDateTimeOptions(Kind = DateTimeKind.Utc)]
        [Required]
        [DataType(DataType.DateTime)]
        public DateTime DateModified { get; set; }

    }
}
