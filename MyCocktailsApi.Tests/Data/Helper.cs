namespace MyCocktailsApi.Tests.Data
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using MyCocktailsApi.Data;
    using MyCocktailsApi.Models;
    using MyCocktailsApi.Services;

    public static class Helper
    {
        private static readonly Random rand = new();

        public static IList<CocktailServiceModel> GetGocktails(int count = 0)
        {
            var cocktailList = new List<CocktailServiceModel>();
            for (int i = 0; i < count; i++)
            {
                var cocktail = GetRandomCocktail(i.ToString());
                cocktailList.Add(cocktail);
            }

            return cocktailList;
        }

        public static CocktailServiceModel GetRandomCocktail(string suffix = "")
        {
            var cocktail = new CocktailServiceModel();
            cocktail.Id = Guid.NewGuid().ToString();
            cocktail.Name = ($"CocktailName {suffix}").TrimEnd();
            cocktail.Category = ($"CategoryName {suffix}").TrimEnd();
            cocktail.Instructions = GetRandomInstructions(suffix);
            cocktail.Likes = rand.Next(0, 10);
            cocktail.UsersLike = GetUserLikes(cocktail.Likes);
            cocktail.Glass = GetRandomGlassType();
            string imageNumber = string.IsNullOrEmpty(suffix) ? "1" : suffix;
            cocktail.PictureUrl = "https://MyRandomImages/" + imageNumber + ".jpg";

            cocktail.Ingredients = GetRandomIngredients();
            cocktail.DateModified = DateTime.UtcNow;

            return cocktail;
        }

        private static string GetRandomGlassType()
        {
            Array values = Enum.GetValues(typeof(GlassType));
            var randomGlassType = (GlassType)values.GetValue(rand.Next(values.Length));

            switch (randomGlassType)
            {
                case GlassType.OldFashioned:
                    return "Old-fashioned";
                case GlassType.CopperMug:
                    return "Copper Mug";
                default:
                    return randomGlassType.ToString();
            }
        }

        private static string GetRandomInstructions(string suffix = "")
        {
            var sb = new StringBuilder();
            for (int i = 0; i < 5; i++)
            {
                sb.Append($"These are my instructions {i} {suffix}. ");
            }

            return sb.ToString().Trim();
        }

        private static IList<InputIngredientModel> GetRandomIngredients()
        {
            var ingredientsList = new List<InputIngredientModel>();

            var ingredientsCount = rand.Next(1, 5);

            for (int i = 0; i < ingredientsCount; i++)
            {
                var ingredient = new InputIngredientModel()
                {
                    Name = Guid.NewGuid().ToString(),
                    Quantity = Guid.NewGuid().ToString()
                };

                ingredientsList.Add(ingredient);
            }

            return ingredientsList;
        }

        private static IList<string> GetUserLikes(int count)
        {
            var userLikesCollection = new List<string>();
            for (int i = 0; i < count; i++)
            {
                userLikesCollection.Add(Guid.NewGuid().ToString());
            }

            return userLikesCollection;
        }
    }
}
