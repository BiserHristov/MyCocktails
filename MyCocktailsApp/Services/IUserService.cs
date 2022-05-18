namespace MyCocktailsApi.Services
{
    using System.Threading.Tasks;

    public interface IUserService
    {
        Task<bool> UserExist(string email);
    }
}
