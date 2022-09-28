using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Portal.Application.Interfaces;
using Portal.Domain.DTOs;
using Portal.Domain.Models;
using Portal.WebApp.Models.UserViewModels;
using Portal.WebApp.Resources;

namespace Portal.WebApp.Controllers
{
    public class AccountController : Controller
    {
        private readonly IApplicationUserManager _applicaionUserManager;

        private readonly IMapper _mapper;

        public AccountController(IApplicationUserManager applicaionUserManager, IMapper mapper)
        {
            _applicaionUserManager = applicaionUserManager;
            _mapper = mapper;
        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(UserLoginViewModel userLogin)
        {
            if (ModelState.IsValid && userLogin != null)
            {
                var userLoginDTO = _mapper.Map<UserLoginDTO>(userLogin);
                var resultLogin = await _applicaionUserManager.WebLogIn(userLoginDTO, userLogin.RememberMe);
                if (resultLogin.Succeeded)
                {
                    return RedirectToAction("Index", "Home");
                }

                ModelState.AddModelError("Email", ValidationErrorMessages.IncorrectUserLoginOrPassword);
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
            if (ModelState.IsValid && userRegister != null)
            {
                var user = _mapper.Map<User>(userRegister);
                var resultRegistration = await _applicaionUserManager.WebRegister(user, userRegister.Password);
                if (resultRegistration.Succeeded)
                {
                    await _applicaionUserManager.SignInAsync(user, false);
                    return RedirectToAction("Index", "Home");
                }

                ModelState.AddModelError("Email", ValidationErrorMessages.UserAlreadyExists);
            }

            return View(userRegister);
        }

        [Authorize]
        public async Task<IActionResult> Logout()
        {
            await _applicaionUserManager.WebLogOff();
            return RedirectToAction("Index", "Home");
        }

        [Authorize]
        public async Task<IActionResult> ShowProfile()
        {
            var user = await _applicaionUserManager.UserRepository.FindByUserName(User.Identity.Name);
            var userWithIncludes = await _applicaionUserManager.UserRepository.FindByIdWithIncludesAsync(user.Id, new string[] { "Skills" });
            var userViewModel = _mapper.Map<UserProfileViewModel>(userWithIncludes);
            return View(userViewModel);
        }
    }
}
