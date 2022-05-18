namespace MyCocktailsApi.Infrastructure
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using System.Threading.Tasks;

    public class CheckDateRangeAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            DateTime dt = (DateTime)value;
            if (dt.Year < 1800 || dt >= DateTime.UtcNow)
            {
                return new ValidationResult(ErrorMessage ?? "Make sure your date is between year 1800 and today");
            }

            return ValidationResult.Success;
        }
    }
}