namespace MyCocktailsApp.Data
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    public class DataConstants
    {
        public class Cocktail
        {
            public const int NameMinLength = 2;
            public const int NameMaxLength = 20;
            public const int CategoryMinLength = 2;
            public const int CategoryMaxLength = 15;
            public const int InstructionsMinLength = 10;
            public const int InstructionsMaxLength = 200;
            public const int MinIngredientsCount = 1;
        }

        public class Ingredient
        {
            public const int NameMinLength = 2;
            public const int NameMaxLength = 20;
            public const int QuantityMinLength = 1;
            public const int QuantityMaxLength = 20;
        }
    }
}
