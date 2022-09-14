using System.ComponentModel.DataAnnotations;

namespace Portal.WebApp.Models.UserViewModels
{
    public class UserLoginViewModel
    {
        [Required]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }

        [Required]
        public bool RememberMe { get; set; }
    }
}
