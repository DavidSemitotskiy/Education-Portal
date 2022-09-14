using Microsoft.AspNetCore.Identity;
using Portal.Application.Interfaces;
using Portal.Domain.DTOs;
using Portal.Domain.Models;
using Portal.WebApp.Models.UserViewModels;

namespace Portal.WebApp.Extensions
{
    public static class UserManagerExtensions
    {
        public static Task<SignInResult> WebLogIn(this IApplicationUserManager userManager, SignInManager<User> signInManager, UserLoginViewModel userLogin)
        {
            return signInManager?.PasswordSignInAsync(userLogin?.Email, userLogin.Password, userLogin.RememberMe, false);
        }

        public static async Task<IdentityResult?> WebRegister(this IApplicationUserManager userManager, UserRegisterViewModel userRegister, User user)
        { 
            return await userManager?.UserManager.CreateAsync(user, userRegister?.Password);
        }

        public static Task WebLogOff(this IApplicationUserManager userManager, SignInManager<User> signInManager)
        {
            return signInManager?.SignOutAsync();
        }
    }
}
