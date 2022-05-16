namespace MyCocktailsApp.Infrastructure
{
    using AutoMapper;
    using MyCocktailsApp.Data.Models;
    using MyCocktailsApp.InputApiModels;
    using MyCocktailsApp.Models;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    public class MappingProfile : Profile
    {
        public MappingProfile()
        {

            this.CreateMap<InputCocktailModel, Cocktail>()
                .ForMember(c => c.Glass, cfg => cfg.MapFrom(m => Enum.Parse<GlassType>(m.Glass)));
        }

        //private static List<Ingredient> DeserializeCollection(InputCocktailModel m)
        //{
        //    var ingrList = new List<Ingredient>();
        //    var list = m.Ingredients.ToList();
        //    for (int i = 0; i < list.Count(); i++)
        //    {
        //        var ingr = new Ingredient();
        //        ingr.Name = list[i].Key;
        //        ingr.Quantity = list[i].Value;
        //        ingrList.Add(ingr);
        //    }

        //    return ingrList;
        //}
    }
}
