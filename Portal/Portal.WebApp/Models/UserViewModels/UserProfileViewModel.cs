using Portal.Domain.Models;

namespace Portal.WebApp.Models.UserViewModels
{
    public class UserProfileViewModel
    {
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Email { get; set; }

        public int AccessLevel { get; set; }

        public List<UserSkill> Skills { get; set; }
    }
}
