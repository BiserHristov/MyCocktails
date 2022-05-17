namespace MyCocktailsApi.Infrastructure
{
    using AutoMapper;
    using MyCocktailsApi.Data.Models;
    using MyCocktailsApi.InputApiModels;
    using MyCocktailsApi.Models;
    using System;

    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            this.CreateMap<InputCocktailModel, Cocktail>()
                .ForMember(c => c.Glass, cfg => cfg.MapFrom(m => Enum.Parse<GlassType>(m.Glass)));

            this.CreateMap<Cocktail, OutputCocktailModel>();
            this.CreateMap<Ingredient, OutputIngredientModel>();
            this.CreateMap<Cocktail, UpdateCocktailModel>();
            this.CreateMap<OutputCocktailModel, UpdateCocktailModel>();
            this.CreateMap<OutputCocktailModel, Cocktail>();
            this.CreateMap<InputIngredientModel, Ingredient>();
            this.CreateMap<OutputIngredientModel, Ingredient>();
            this.CreateMap<CocktailApiModel, InputCocktailModel>()
                .ForMember(c => c.Glass, cfg => cfg.MapFrom(m => m.GlassType.ToString()));

        }
    }
}
