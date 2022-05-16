namespace MyCocktailsApp.Infrastructure
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using System.Threading.Tasks;

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
