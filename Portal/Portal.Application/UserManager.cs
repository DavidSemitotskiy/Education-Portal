using Portal.Application.Interfaces;
using Portal.Domain.DTOs;
using Portal.Domain.Interfaces;
using Portal.Domain.Models;

namespace Portal.Application
{
    public class UserManager : IUserManager
    {
        public UserManager(IUserRepository repository)
        {
            UserRepository = repository ?? throw new ArgumentNullException("Repository can't be null");
        }

        public IUserRepository UserRepository { get; }

        public async Task<bool> Exists(UserRegisterDTO userRegister)
        {
            var allUsers = await UserRepository.GetAllUsers();
            return allUsers.Any(user => user.Email == userRegister.Email);
        }

        public async Task<User> GetLogInUser(UserLoginDTO userLogIn)
        {
            if (userLogIn == null)
            {
                throw new ArgumentNullException("User can't be null");
            }

            var allUsers = await UserRepository.GetAllUsers();
            var hashedPassword = AccountService.GetHashPassword(userLogIn.Password);
            return allUsers.FirstOrDefault(user => user.Email == userLogIn.Email && user.Password == hashedPassword);
        }

        public async Task LogIn(UserLoginDTO userLogin)
        {
            var logInUser = await GetLogInUser(userLogin);
            if (logInUser == null)
            {
                throw new Exception("User isn't registered");
            }

            logInUser.Skills = logInUser.Skills ?? new List<UserSkill>();
            logInUser.OwnCourses = logInUser.OwnCourses ?? new List<Course>();
            IUserManager.CurrentUser = logInUser;
        }

        public async Task LogOff()
        {
            IUserManager.CurrentUser = null;
        }

        public async Task Register(UserRegisterDTO userRegister)
        {
            if (userRegister == null)
            {
                throw new ArgumentNullException("User can't be null");
            }

            var isUserExists = await Exists(userRegister);
            if (isUserExists)
            {
                throw new ArgumentException("User with the same name already exists");
            }

            var user = new User
            {
                UserId = Guid.NewGuid(),
                FirstName = userRegister.FirstName,
                LastName = userRegister.LastName,
                Password = AccountService.GetHashPassword(userRegister.Password),
                Email = userRegister.Email,
                AccessLevel = 0,
                Skills = new List<UserSkill>()
            };
            await UserRepository.Add(user);
            IUserManager.CurrentUser = user;
        }
    }
}
