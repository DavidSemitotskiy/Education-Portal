using Portal.Domain.Models;

namespace Portal.Domain.Interfaces
{
    public interface IUserRepository
    {
        Task<List<User>> GetAllUsers();

        Task Add(User user);

        void Delete(User user);

        void Update(User user);

        Task<int> SaveChanges();
    }
}
