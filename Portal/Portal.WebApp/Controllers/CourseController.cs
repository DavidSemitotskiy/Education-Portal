using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Portal.Application.Interfaces;
using Portal.Domain.Models;
using Portal.WebApp.Models.CourseViewModels;

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

        public async Task<IActionResult> CourseConstructor()
        {
            var user = await _applicationUserManager.UserRepository.FindByUserName(User.Identity.Name);
            return View(await _courseManager.GetOwnCourses(user));
        }

        public async Task<IActionResult> MyCourses()
        {
            var user = await _applicationUserManager.UserRepository.FindByUserName(User.Identity.Name);
            return View(await _courseManager.GetCoursesInProgress(user));
        }

        public IActionResult CreateCourse()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateCourse(CreateCourseViewModel createdCourse)
        {
            if (ModelState.IsValid)
            {
                var courseToCreate = new Course
                {
                    Id = Guid.NewGuid(),
                    Name = createdCourse?.Name,
                    Description = createdCourse.Description,
                    AccessLevel = createdCourse.AccessLevel,
                    IsPublished = false,
                    OwnerUser = User.Identity.Name
                };
                await _courseManager.AddCourse(courseToCreate);
                await _courseManager.CourseRepository.SaveChanges();
                return RedirectToAction("CourseConstructor");
            }

            return View(createdCourse);
        }
    }
}
