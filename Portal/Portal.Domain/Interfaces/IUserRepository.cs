using Portal.Domain.Models;
using Portal.Domain.Specifications;

namespace Portal.Domain.Interfaces
{
    public interface IUserRepository
    {
        Task<List<User>> GetAllUsers();

        Task<User> FindById(Guid id);

        Task<User> FindByIdWithIncludesAsync(Guid id, string[] includeNames);

        Task<List<User>> FindUsersBySpecification(Specification<User> specification);

        Task Add(User user);

        void Delete(User user);

        void Update(User user);

        Task<int> SaveChanges();
    }
}
