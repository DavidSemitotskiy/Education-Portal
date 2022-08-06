using Portal.Application.Interfaces;
using Portal.Domain.DTOs;
using Portal.Domain.Interfaces;
using Portal.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Portal.Application
{
    internal class UserManager : IUserManager
    {
        public UserManager(IUserRepository userRepository)
        {
            UserRepository = userRepository ?? throw new ArgumentNullException("Repository can't be null");
        }

        public IUserRepository UserRepository { get; set; }

        public User? CurrentUser { get; set; }

        public void Register(UserRegisterDTO userRegister)
        {
            if (userRegister == null)
            {
                throw new ArgumentNullException("New User can't be null");
            }

            if (UserRepository.Exists(userRegister.FirstName, userRegister.LastName, userRegister.Email))
            {
                throw new ArgumentException("User with the same name already exists");
            }

            if (!userRegister.Password.Equals(userRegister.ConfirmPassword))
            {
                throw new ArgumentException("Confirm password doesn't match to Password");
            }

            AccountService accountService = new AccountService();
            User user = new User
            {
                IdUser = Guid.NewGuid(),
                FirstName = userRegister.FirstName,
                LastName = userRegister.LastName,
                Password = accountService.GetHashPassword(userRegister.Password),
                Email = userRegister.Email,
                AccessLevel = 0,
                Skills = new List<UserSkill>()
            };

            UserRepository.Add(user);
            CurrentUser = user;
        }

        public void LogIn(UserLoginDTO userLogin)
        {
            if (userLogin == null)
            {
                throw new ArgumentNullException("New User can't be null");
            }

            AccountService accountService = new AccountService();
            userLogin.Password = accountService.GetHashPassword(userLogin.Password);
            User logInUser = UserRepository.GetLogInUser(userLogin);
            if (logInUser == null)
            {
                throw new Exception("User isn't registered");
            }

            CurrentUser = logInUser;
        }

        public void LogOff()
        {
            if (CurrentUser == null)
            {
                throw new ArgumentException("User isn't authorized");
            }

            CurrentUser = null;
        }
    }
}
