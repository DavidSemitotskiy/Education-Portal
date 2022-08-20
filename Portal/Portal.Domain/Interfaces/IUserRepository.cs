using Portal.Domain.Models;

namespace Portal.Domain.Interfaces
{
    public interface IUserRepository
    {
        Task<IEnumerable<User>> GetAllUsers();

        Task Add(User user);

        Task Delete(User user);

        Task Update(User user);

        Task SaveChanges();
    }
}
