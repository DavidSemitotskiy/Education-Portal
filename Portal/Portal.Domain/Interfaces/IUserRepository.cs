using Portal.Domain.Models;
using Portal.Domain.Specifications;

namespace Portal.Domain.Interfaces
{
    public interface IUserRepository
    {
        Task<List<User>> GetAllUsers();

        Task<User> FindById(string id);

        Task<User> FindByUserName(string userName);

        Task<User> FindByIdWithIncludesAsync(string id, string[] includeNames);

        Task<List<User>> FindUsersBySpecification(Specification<User> specification);

        Task Add(User user);

        void Delete(User user);

        void Update(User user);

        Task<int> SaveChanges();
    }
}
