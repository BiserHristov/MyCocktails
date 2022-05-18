namespace MyCocktailsApi.Models
{
    using System.ComponentModel.DataAnnotations;

    public class InputLoginModel
    {
        [Required]
        [EmailAddress(ErrorMessage = "Email address should be valid!")]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }
    }
}
