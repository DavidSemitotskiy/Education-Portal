﻿using Portal.Application.Interfaces;
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

            var taskAllUsers = UserRepository.GetAllUsers();
            var hashedPassword = AccountService.GetHashPassword(userLogIn.Password);
            var allUsers = await taskAllUsers;
            return allUsers.FirstOrDefault(user => user.Email == userLogIn.Email && user.Password == hashedPassword);
        }

        public async Task LogIn(UserLoginDTO userLogin)
        {
            var logInUser = await GetLogInUser(userLogin);
            if (logInUser == null)
            {
                throw new Exception("User isn't registered");
            }

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

            if (!userRegister.Password.Equals(userRegister.ConfirmPassword))
            {
                throw new ArgumentException("Confirm password doesn't match to Password");
            }

            var isUserExists = await Exists(userRegister);
            if (isUserExists)
            {
                throw new ArgumentException("User with the same name already exists");
            }

            var user = new User
            {
                IdUser = Guid.NewGuid(),
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
