namespace MyCocktailsApi.Services
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using MyCocktailsApi.Models;

    public interface ICocktailService
    {
        Task<IEnumerable<OutputCocktailModel>> GetAllAsync();

        Task<OutputCocktailModel> GetByIdAsync(string id);

        Task<OutputCocktailModel> GetByNameAsync(string name);

        Task<IEnumerable<OutputCocktailModel>> GetByCategoryAsync(string category);

        Task<OutputCocktailModel> CreateAsync(InputCocktailModel cocktail);

        Task UpdateAsync(OutputCocktailModel dbCoctail, InputCocktailModel updatedCocktail);

        Task RemoveAsync(string id);

        Task UpdateLikes(OutputCocktailModel model, string userId);
    }
}
