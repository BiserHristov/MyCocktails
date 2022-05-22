namespace MyCocktailsApi.Data
{
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
            public const int NameMaxLength = 24;
            public const int QuantityMinLength = 1;
            public const int QuantityMaxLength = 20;
        }

        public class User
        {
            public const int NameMinLength = 2;
            public const int NameMaxLength = 20;
        }
    }
}
