using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Portal.Application.Interfaces;

namespace Portal.WebApp.Controllers
{
    [Authorize]
    public class CourseController : Controller
    {
        private readonly ICourseManager _courseManager;

        private readonly IApplicationUserManager _applicationUserManager;

        public CourseController(ICourseManager courseManager, IApplicationUserManager applicationUserManager)
        {
            _courseManager = courseManager;
            _applicationUserManager = applicationUserManager;
        }

        public async Task<IActionResult> Index()
        {
            var user = await _applicationUserManager.UserRepository.FindByUserName(User.Identity.Name);
            return View(await _courseManager.GetOwnCourses(user));
        }
    }
}
