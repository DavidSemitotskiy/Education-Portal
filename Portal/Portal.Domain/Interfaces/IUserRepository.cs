using Portal.Domain.DTOs;
using Portal.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Portal.Domain.Interfaces
{
    public interface IUserRepository
    {
        IEnumerable<User> GetAllUsers();

        void Add(User user);

        bool Exists(string firstName, string lastName, string email);

        User GetLogInUser(UserLoginDTO user);
    }
}
