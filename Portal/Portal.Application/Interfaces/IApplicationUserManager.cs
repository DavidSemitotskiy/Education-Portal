using Microsoft.AspNetCore.Identity;
using Portal.Domain.DTOs;
using Portal.Domain.Interfaces;
using Portal.Domain.Models;

namespace Portal.Application.Interfaces
{
    public interface IApplicationUserManager
    {
        static User? CurrentUser { get; set; }

        IUserRepository UserRepository { get; }

        UserManager<User> UserManager { get; }

        Task<User> GetLogInUser(UserLoginDTO userLogIn);

        Task<bool> Exists(UserRegisterDTO userRegister);

        Task ConsoleLogIn(UserLoginDTO userLogin);

        Task ConsoleRegister(UserRegisterDTO userRegister);

        Task ConsoleLogOff();

        Task<SignInResult> WebLogIn(UserLoginDTO userLogin, bool rememberMe);

        Task<IdentityResult?> WebRegister(User user, string password);

        Task WebLogOff();

        Task SignInAsync(User user, bool isPersistent);
    }
}
