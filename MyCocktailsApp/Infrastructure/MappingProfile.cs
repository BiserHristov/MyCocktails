namespace MyCocktailsApi.Infrastructure
{
    using System;
    using System.Linq;
    using System.Text;
    using AutoMapper;
    using MyCocktailsApi.Data.Models;
    using MyCocktailsApi.InputApiModels;
    using MyCocktailsApi.Models;

    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            this.CreateMap<InputCocktailModel, Cocktail>()
                .ForMember(c => c.Glass, cfg => cfg.MapFrom(m => Enum.Parse<GlassType>(RemoveSymbols(m.Glass))));

            this.CreateMap<Cocktail, OutputCocktailModel>();
            this.CreateMap<Ingredient, OutputIngredientModel>();
            this.CreateMap<OutputCocktailModel, Cocktail>();
            this.CreateMap<InputIngredientModel, Ingredient>();
            this.CreateMap<OutputIngredientModel, Ingredient>();
            this.CreateMap<OutputIngredientModel, InputIngredientModel>();
            this.CreateMap<InputIngredientModel, OutputIngredientModel>();
            this.CreateMap<CocktailApiModel, InputCocktailModel>()
                .ForMember(c => c.Glass, cfg => cfg.MapFrom(m => m.GlassType.ToString()));
        }

        public static string RemoveSymbols(string input)
        {
            var sb = new StringBuilder();
            var symbols = new char[] { ' ', '-' };
            var inputAsCharArr = input.ToCharArray();

            if (!inputAsCharArr.Any(x => symbols.Contains(x)))
            {
                return input;
            }

            var previousCharIsSymbol = false;

            for (int i = 0; i < inputAsCharArr.Length; i++)
            {
                var letter = inputAsCharArr[i];

                if (symbols.Contains(letter))
                {
                    previousCharIsSymbol = true;
                    continue;
                }

                if (previousCharIsSymbol)
                {
                    letter = char.ToUpper(letter);
                    previousCharIsSymbol = false;
                }

                sb.Append(letter);
            }

            return sb.ToString().Trim();
        }
    }
}
