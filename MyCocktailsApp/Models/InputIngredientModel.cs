namespace MyCocktailsApi.Models
{
    using MyCocktailsApi.Infrastructure;
    using System.ComponentModel.DataAnnotations;

    using static MyCocktailsApi.Data.DataConstants.Ingredient;
    public class InputIngredientModel
    {
        [Required]
        [StringLength(NameMaxLength, MinimumLength = NameMinLength)]
        public string Name { get; set; }

        [ValidIfExist]
        public string? Quantity { get; set; }
    }
}
