namespace MyCocktailsApi.Infrastructure
{
    using System.ComponentModel.DataAnnotations;

    using static MyCocktailsApi.Data.DataConstants.Ingredient;

    public class ValidIfExistAttribute : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            var quantityAsString = value as string;
            if (string.IsNullOrWhiteSpace(quantityAsString))
            {
                return true;
            }
            else
            {
                if (quantityAsString.Length >= QuantityMinLength &&
                    quantityAsString.Length <= QuantityMaxLength)
                {
                    return true;
                }

                return false;
            }
        }
    }
}
