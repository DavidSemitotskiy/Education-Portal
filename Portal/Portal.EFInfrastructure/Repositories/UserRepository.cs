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
