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
            var coursesInProgress = await _courseManager.GetCoursesInProgress(user);
            List<CourseStateViewModel> courseStateViewModels = new List<CourseStateViewModel>();
            Course course = null;
            foreach (var courseState in coursesInProgress)
            {
                course = await _courseManager.CourseRepository.FindById(courseState.CourseId);
                courseStateViewModels.Add(new CourseStateViewModel
                {
                    Id = courseState.Id,
                    CourseId = courseState.CourseId,
                    Name = course.Name,
                    Description = course.Description,
                    Progress = await _courseManager.CourseStateManager.GetCourseProgress(courseState)
                });
            }

            return View(courseStateViewModels);
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

        public async Task<IActionResult> EditCourse(Guid id)
        {
            var courseToEdit = await _courseManager.CourseRepository.FindByIdWithIncludesAsync(id, new string[] { "Materials", "Skills" });
            if (courseToEdit != null)
            {
                var editCourseViewModel = new EditCourseViewModel
                {
                    Id = courseToEdit.Id,
                    Name = courseToEdit.Name,
                    Description = courseToEdit.Description,
                    AccessLevel = courseToEdit.AccessLevel,
                    IsPublished = courseToEdit.IsPublished,
                    Materials = courseToEdit.Materials,
                    Skills = courseToEdit.Skills,
                };
                return View(editCourseViewModel);
            }

            return RedirectToAction("CourseConstructor");
        }

        public async Task<IActionResult> DeleteCourse(Guid id)
        {
            var courseToDelete = await _courseManager.CourseRepository.FindById(id);
            if (courseToDelete != null && !courseToDelete.IsPublished)
            {
                _courseManager.DeleteCourse(courseToDelete);
                await _courseManager.CourseRepository.SaveChanges();
            }

            return RedirectToAction("CourseConstructor");
        }
    }
}
