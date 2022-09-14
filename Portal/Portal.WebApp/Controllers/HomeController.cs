using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Portal.Application.Interfaces;
using Portal.WebApp.Models;
using System.Diagnostics;

namespace Portal.WebApp.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        private readonly ICourseManager _courseManager;

        private readonly IApplicationUserManager _applicationUserManager;

        public HomeController(ILogger<HomeController> logger, ICourseManager courseManager, IApplicationUserManager applicationUserManager)
        {
            _logger = logger;
            _courseManager = courseManager;
            _applicationUserManager = applicationUserManager;
        }

        public async Task<IActionResult> Index()
        {
            var user = await _applicationUserManager.UserRepository.FindByUserName(User.Identity.Name);
            return View(await _courseManager.GetAvailableCourses(user));
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}