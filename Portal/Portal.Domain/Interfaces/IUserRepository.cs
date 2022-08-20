using Portal.Domain.Models;

namespace Portal.Domain.Interfaces
{
    public interface IUserRepository
    {
        Task<List<User>> GetAllUsers();

        Task Add(User user);

        Task Delete(User user);

        Task Update(User user);

        Task SaveChanges();
    }
}
