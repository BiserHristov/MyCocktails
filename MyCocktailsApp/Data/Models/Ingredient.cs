namespace MyCocktailsApi.Data.Models
{
    using System.ComponentModel.DataAnnotations;
    using MyCocktailsApi.Infrastructure;
    using static MyCocktailsApi.Data.DataConstants.Ingredient;

    public class Ingredient
    {
        [Required]
        [StringLength(NameMaxLength, MinimumLength = NameMinLength)]
        public string Name { get; set; }

        [ValidIfExist]
        public string Quantity { get; set; }
    }
}