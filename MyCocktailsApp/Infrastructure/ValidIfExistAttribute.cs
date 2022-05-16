namespace MyCocktailsApp.Infrastructure
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using System.Threading.Tasks;

    using static MyCocktailsApp.Data.DataConstants.Ingredient;

    public class ValidIfExistAttribute : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            var quantityAsString = value as string;
            if (quantityAsString == null)
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
