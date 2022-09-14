using Microsoft.AspNetCore.Identity;
using Portal.Application.Interfaces;
using Portal.Application.Specifications.UserSpecifications;
using Portal.Domain.DTOs;
using Portal.Domain.Interfaces;
using Portal.Domain.Models;

namespace Portal.Application
{
    public class ApplicationUserManager : IApplicationUserManager
    {
        public ApplicationUserManager(IUserRepository repository)
        {
            UserRepository = repository ?? throw new ArgumentNullException("Repository can't be null");
        }

        public ApplicationUserManager(IUserRepository repository, UserManager<User> userManager) : this(repository)
        {
            UserManager = userManager ?? throw new ArgumentNullException("Manager can't be null");
        }

        public IUserRepository UserRepository { get; }

        public UserManager<User> UserManager { get; }

        public async Task<bool> Exists(UserRegisterDTO userRegister)
        {
            var existsUserRegisterSpecification = new ExistsUserRegisterSpecification(userRegister);
            var count = (await UserRepository.FindUsersBySpecification(existsUserRegisterSpecification)).Count();
            return Convert.ToBoolean(count);
        }

        public async Task<User> GetLogInUser(UserLoginDTO userLogIn)
        {
            if (userLogIn == null)
            {
                throw new ArgumentNullException("User can't be null");
            }

            var existsUserLogInSpecification = new ExistsUserLogInSpecification(userLogIn);
            return (await UserRepository.FindUsersBySpecification(existsUserLogInSpecification)).FirstOrDefault();
        }

        public async Task ConsoleLogIn(UserLoginDTO userLogin)
        {
            var logInUser = await GetLogInUser(userLogin);
            if (logInUser == null)
            {
                throw new Exception("User isn't registered");
            }

            IApplicationUserManager.CurrentUser = logInUser;
        }

        public async Task ConsoleLogOff()
        {
            IApplicationUserManager.CurrentUser = null;
        }

        public async Task ConsoleRegister(UserRegisterDTO userRegister)
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
                Id = Guid.NewGuid().ToString(),
                FirstName = userRegister.FirstName,
                LastName = userRegister.LastName,
                PasswordHash = AccountService.GetHashPassword(userRegister.Password),
                Email = userRegister.Email,
                UserName = userRegister.Email,
                AccessLevel = 0,
                Skills = new List<UserSkill>()
            };

            await UserRepository.Add(user);
            IApplicationUserManager.CurrentUser = user;
        }
    }
}
