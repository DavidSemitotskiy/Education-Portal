using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Portal.Domain.Models
{
    public class User
    {
        public Guid IdUser { get; set; }

        public string UserName { get; set; }

        public string Password { get; set; }

        public string Email { get; set; }

        public int AccessLevel { get; set; }

        public List<UserSkill>? Skills { get; set; }
    }
}
