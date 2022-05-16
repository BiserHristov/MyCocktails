namespace MyCocktailsApp.Services
{
    using MyCocktailsApp.Data.Models;
    using MyCocktailsApp.Models;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface ICocktailService
    {
        Task<List<Cocktail>> GetAllAsync();
        Task<Cocktail> GetByIdAsync(string id);
        Task<Cocktail> GetByNameAsync(string name);
        Task<IEnumerable<Cocktail>> GetByCategoryAsync(string category);
        Task<Cocktail> CreateAsync(InputCocktailModel cocktail);
        Task UpdateAsync(string id, Cocktail cocktail);
        Task RemoveAsync(string id);
    }
}
