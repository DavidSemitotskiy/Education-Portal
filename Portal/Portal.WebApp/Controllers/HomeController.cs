using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Portal.Application.Interfaces;
using Portal.WebApp.Models;
using Portal.WebApp.Models.CourseViewModels;
using System.Diagnostics;
using Portal.WebApp.Extensions;
using Portal.WebApp.Filters;
using AutoMapper;

namespace Portal.WebApp.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly ICourseManager _courseManager;

        private readonly IApplicationUserManager _applicationUserManager;

        private readonly IMapper _mapper;

        public HomeController(ICourseManager courseManager, IApplicationUserManager applicationUserManager, IMapper mapper)
        {
            _courseManager = courseManager;
            _applicationUserManager = applicationUserManager;
            _mapper = mapper;
        }

        [TypeFilter(typeof(PaginationFilterAttribute))]
        public async Task<IActionResult> Index(string searchString, int pageNumber, int pageSize)
        {
            ViewData["SearchString"] = searchString;
            var user = await _applicationUserManager.UserRepository.FindByUserName(User.Identity.Name);
            var availableCoursesByPage = await _courseManager.GetAvailableCoursesByPageWithSearchString(user, searchString, pageNumber, pageSize);
            var courseViewModels = _mapper.Map<List<CourseViewModel>>(availableCoursesByPage);
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