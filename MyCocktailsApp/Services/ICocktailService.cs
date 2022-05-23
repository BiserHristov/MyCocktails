namespace MyCocktailsApi.Services
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using MyCocktailsApi.Data.Models;
    using MyCocktailsApi.Models;

    public interface ICocktailService
    {
        Task<IEnumerable<OutputCocktailModel>> GetAllAsync();

        Task<OutputCocktailModel> GetByIdAsync(string id);

        Task<OutputCocktailModel> GetByNameAsync(string name);

        Task<IEnumerable<OutputCocktailModel>> GetByCategoryAsync(string category);

        Task<OutputCocktailModel> CreateAsync(InputCocktailServiceModel cocktail);

        Task UpdateAsync(OutputCocktailModel dbCoctail, InputCocktailModel updatedCocktail);

        Task DeleteAsync(string id);

        Task UpdateLikes(OutputCocktailModel model, string userId);
    }
}
