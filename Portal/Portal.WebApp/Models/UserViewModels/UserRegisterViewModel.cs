using System.ComponentModel.DataAnnotations;

namespace Portal.WebApp.Models.UserViewModels
{
    public class UserRegisterViewModel
    {
        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }

        [Required]
        public string Password { get; set; }

        [Required]
        [Compare("Password", ErrorMessage = "ConfirmPassword must be equal Password")]
        public string ConfirmPassword { get; set; }

        [Required]
        public string Email { get; set; }
    }
}
