
namespace MyCocktailsApi.Data.Models
{
    using AspNetCore.Identity.MongoDbCore.Models;
    using MongoDbGenericRepository.Attributes;
    using System;

    [CollectionName("Users")]
    public class ApplicationUser : MongoIdentityUser<Guid>
    {
    }
}
