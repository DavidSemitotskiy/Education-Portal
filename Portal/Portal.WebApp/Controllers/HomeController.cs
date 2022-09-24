using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Portal.Application.Interfaces;
using Portal.WebApp.Models;
using Portal.WebApp.Models.CourseViewModels;
using System.Diagnostics;
using Portal.WebApp.Extensions;

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

        public async Task<IActionResult> Index(string searchString, int pageNumber = 1, int pageSize = 2)
        {
            ViewData["SearchString"] = searchString;
            var user = await _applicationUserManager.UserRepository.FindByUserName(User.Identity.Name);
            var availableCoursesByPage = await _courseManager.GetAvailableCoursesByPageWithSearchString(user, searchString, pageNumber, pageSize);
            var courseViewModels = availableCoursesByPage.Select(course => new CourseViewModel
            {
                Id = course.Id,
                Description = course.Description,
                Name = course.Name
            }).ToList();
            var page = this.GetPage(courseViewModels, await _courseManager.TotalCountOfAvailableCoursesWithSearchString(user, searchString), pageNumber, pageSize);
            return View(page);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}