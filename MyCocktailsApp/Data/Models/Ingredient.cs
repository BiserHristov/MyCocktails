using MyCocktailsApp.Infrastructure;
using System.ComponentModel.DataAnnotations;
using static MyCocktailsApp.Data.DataConstants.Ingredient;

namespace MyCocktailsApp.Data.Models
{

    public class Ingredient
    {
        [Required]
        [StringLength(NameMaxLength, MinimumLength = NameMinLength)]
        public string Name { get; set; }

        [ValidIfExist]
        public string? Quantity { get; set; }

    }
}
