using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Portal.Domain.Interfaces;
using Portal.Domain.Models;

namespace Portal.EFInfrastructure.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly Context _context;

        public UserRepository(Context context)
        {
            _context = context ?? throw new ArgumentNullException("Context can't be null");
        }

        public Task<User> FindById(Guid id)
        {
            return _context.Users.FirstOrDefaultAsync(user => user.UserId == id);
        }

        public Task<User> FindByIdWithIncludesAsync(Guid id, string[] includeNames)
        {
            if (includeNames == null)
            {
                throw new ArgumentNullException("Include names can't be null");
            }

            IQueryable<User> query = _context.Users;
            foreach (var includeName in includeNames)
            {
                query = query.Include(includeName);
            }

            return query.FirstOrDefaultAsync(user => user.UserId == id);
        }

        public Task Add(User user)
        {
            _context.Users.Add(user);
            return Task.CompletedTask;
        }

        public void Delete(User user)
        {
            _context.Users.Remove(user);
        }

        public Task<List<User>> GetAllUsers()
        {
            return _context.Users.ToListAsync();
        }

        public void Update(User user)
        {
            _context.Users.Update(user);
        }

        public Task<int> SaveChanges()
        {
            return _context.SaveChangesAsync();
        }
    }
}
