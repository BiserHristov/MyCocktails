using MyCocktailsApi.Infrastructure;
using System.ComponentModel.DataAnnotations;
using static MyCocktailsApi.Data.DataConstants.Ingredient;

namespace MyCocktailsApi.Data.Models
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
