using Portal.Domain.DTOs;
using Portal.Domain.Models;

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
