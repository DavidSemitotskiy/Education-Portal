using Portal.Domain.DTOs;
using Portal.Domain.Interfaces;
using Portal.Domain.Models;

namespace Portal.Application.Interfaces
{
    public interface IUserManager
    {
        static User? CurrentUser { get; set; }

        IUserRepository UserRepository { get; }

        Task<User> GetLogInUser(UserLoginDTO userLogIn);

        Task<bool> Exists(UserRegisterDTO userRegister);

        Task LogIn(UserLoginDTO userLogin);

        Task Register(UserRegisterDTO userRegister);

        Task LogOff();
    }
}
