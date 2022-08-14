using Microsoft.EntityFrameworkCore;
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
            await _context.SaveChangesAsync();
        }

        public async Task Delete(User user)
        {
            var userToDelete = await _context.Users.FirstOrDefaultAsync(u => u.UserId == user.UserId);
            if (userToDelete != null)
            {
                _context.Users.Remove(userToDelete);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<User>> GetAllUsers()
        {
            var allUsers = await _context.Users.ToListAsync();
            return allUsers;
        }

        public async Task Update(User user)
        {
            var userToUpdate = await _context.Users.FirstOrDefaultAsync(u => u.UserId == user.UserId);
            if (userToUpdate != null)
            {
                userToUpdate = user;
                _context.Users.Update(userToUpdate);
                await _context.SaveChangesAsync();
            }
        }
    }
}
