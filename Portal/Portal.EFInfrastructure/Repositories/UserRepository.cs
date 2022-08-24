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

        public async Task Add(User user)
        {
            await _context.Users.AddAsync(user);
        }

        public Task Delete(User user)
        {
            EntityEntry userEntry = _context.Entry(user);
            userEntry.State = EntityState.Deleted;
            _context.Users.Remove(user);
            return Task.CompletedTask;
        }

        public Task<List<User>> GetAllUsers()
        {
            return _context.Users.ToListAsync();
        }

        public Task Update(User user)
        {
            EntityEntry userEntry = _context.Entry(user);
            userEntry.State = EntityState.Modified;
            _context.Users.Update(user);
            return Task.CompletedTask;
        }

        public Task<int> SaveChanges()
        {
            return _context.SaveChangesAsync();
        }
    }
}
