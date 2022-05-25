namespace MyCocktailsApi.Services
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using MyCocktailsApi.Data.Models;
    using MyCocktailsApi.Models;

    public interface ICocktailService
    {
        Task<IEnumerable<CocktailServiceModel>> GetAllAsync();

        Task<CocktailServiceModel> GetByIdAsync(string id);

        Task<CocktailServiceModel> GetByNameAsync(string name);

        Task<IEnumerable<CocktailServiceModel>> GetByCategoryAsync(string category);

        Task<CocktailServiceModel> CreateAsync(CocktailServiceModel cocktail);

        Task UpdateAsync(CocktailServiceModel dbCoctail, CocktailServiceModel updatedCocktail);

        Task DeleteAsync(string id);

        Task UpdateLikes(CocktailServiceModel model, string userId);
    }
}
