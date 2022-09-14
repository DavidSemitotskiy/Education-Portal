using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Portal.Application.Interfaces;
using Portal.Domain.Models;
using Portal.WebApp.Extensions;
using Portal.WebApp.Models.UserViewModels;

namespace Portal.WebApp.Controllers
{
    public class AccountController : Controller
    {
        private readonly IApplicationUserManager _applicaionUserManager;

        private readonly SignInManager<User> _signInManager;

        public AccountController(IApplicationUserManager applicaionUserManager, SignInManager<User> signInManager)
        {
            _applicaionUserManager = applicaionUserManager;
            _signInManager = signInManager;
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
                var resultLogin = await _applicaionUserManager.WebLogIn(_signInManager, userLogin);
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
                var resultRegistration = await _applicaionUserManager.WebRegister(userRegister, user);
                if (resultRegistration.Succeeded)
                {
                    _signInManager.SignInAsync(user, false);
                    return RedirectToAction("Index", "Home");
                }

                ModelState.AddModelError("Email", "User with this email already exists");
            }

            return View(userRegister);
        }

        [Authorize]
        public async Task<IActionResult> Logout()
        {
            await _applicaionUserManager.WebLogOff(_signInManager);
            return RedirectToAction("Index", "Home");
        }
    }
}
