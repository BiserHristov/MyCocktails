namespace MyCocktailsApi.Data.Models
{
    using System.ComponentModel.DataAnnotations;

    using static MyCocktailsApi.Data.DataConstants.User;
    public class User
    {
        [Required]
        [StringLength(NameMaxLength, MinimumLength = NameMinLength)]
        public string Name { get; set; }

        [Required]
        [EmailAddress(ErrorMessage = "Invalid Email Address")]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }
    }
}
