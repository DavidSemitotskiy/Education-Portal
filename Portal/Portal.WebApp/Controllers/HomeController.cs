using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Portal.Application.Interfaces;
using Portal.WebApp.Models;
using Portal.WebApp.Models.CourseViewModels;
using System.Diagnostics;
using Portal.WebApp.Extensions;
using Portal.WebApp.Filters;

namespace Portal.WebApp.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly ICourseManager _courseManager;

        private readonly IApplicationUserManager _applicationUserManager;

        public HomeController(ICourseManager courseManager, IApplicationUserManager applicationUserManager)
        {
            _courseManager = courseManager;
            _applicationUserManager = applicationUserManager;
        }

        [TypeFilter(typeof(PaginationFilterAttribute))]
        public async Task<IActionResult> Index(string searchString, int pageNumber, int pageSize)
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