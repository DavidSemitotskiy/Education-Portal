using Microsoft.AspNetCore.Identity;

namespace Portal.Domain.Models
{
    public class User : IdentityUser
    {
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public int AccessLevel { get; set; }

        public List<UserSkill> Skills { get; set; }
    }
}
