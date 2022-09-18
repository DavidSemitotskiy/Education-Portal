using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Portal.Application.Interfaces;
using Portal.Domain.DTOs;
using Portal.Domain.Models;
using Portal.WebApp.Models.UserViewModels;

namespace Portal.WebApp.Controllers
{
    public class AccountController : Controller
    {
        private readonly IApplicationUserManager _applicaionUserManager;

        public AccountController(IApplicationUserManager applicaionUserManager)
        {
            _applicaionUserManager = applicaionUserManager;
        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(UserLoginViewModel userLogin)
        {
            if (ModelState.IsValid)
            {
                var resultLogin = await _applicaionUserManager.WebLogIn(new UserLoginDTO
                {
                    Email = userLogin?.Email,
                    Password = userLogin.Password,
                }, userLogin.RememberMe);
                if (resultLogin.Succeeded)
                {
                    return RedirectToAction("Index", "Home");
                }

                ModelState.AddModelError("Email", "Incorrect Email or Password");
            }

            return View(userLogin);
        }

        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(UserRegisterViewModel userRegister)
        {
            if (ModelState.IsValid)
            {
                var user = new User
                {
                    FirstName = userRegister?.FirstName,
                    LastName = userRegister.LastName,
                    Email = userRegister.Email,
                    UserName = userRegister.Email,
                    AccessLevel = 0,
                    Skills = new List<UserSkill>()
                };
                var resultRegistration = await _applicaionUserManager.WebRegister(user, userRegister.Password);
                if (resultRegistration.Succeeded)
                {
                    await _applicaionUserManager.SignInAsync(user, false);
                    return RedirectToAction("Index", "Home");
                }

                ModelState.AddModelError("Email", "User with this email already exists");
            }

            return View(userRegister);
        }

        [Authorize]
        public async Task<IActionResult> Logout()
        {
            await _applicaionUserManager.WebLogOff();
            return RedirectToAction("Index", "Home");
        }
    }
}
