namespace MyCocktailsApi.Infrastructure
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;

    public class NotEmptyCollectionAttribute : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            var collection = value as IEnumerable<object>;
            if (collection == null || collection.Count() == 0)
            {
                return false;
            }

            return true;
        }
    }
}
