using Portal.Domain.DTOs;
using Portal.Domain.Interfaces;
using Portal.Domain.Models;

namespace Portal.Application.Interfaces
{
    public interface IUserManager
    {
        static User? CurrentUser { get; set; }

        IUserRepository UserRepository { get; set; }

        void LogIn(UserLoginDTO userLogin);

        void Register(UserRegisterDTO userRegister);

        void LogOff();
    }
}
