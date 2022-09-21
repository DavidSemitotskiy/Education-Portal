using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NToastNotify;
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

        private IToastNotification _toastNotification;

        public CourseController(ICourseManager courseManager, IApplicationUserManager applicationUserManager, IToastNotification toastNotification)
        {
            _courseManager = courseManager;
            _applicationUserManager = applicationUserManager;
            _toastNotification = toastNotification;
        }

        public async Task<IActionResult> CourseConstructor()
        {
            var user = await _applicationUserManager.UserRepository.FindByUserName(User.Identity.Name);
            var ownCourses = await _courseManager.GetOwnCourses(user);
            return View(ownCourses.Select(course => new CourseViewModel
            {
                Id = course.Id,
                Name = course.Name,
                Description = course.Description,
                IsPublished = course.IsPublished
            }).ToList());
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
                _toastNotification.AddSuccessToastMessage("Course was successfully created");
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

        [HttpPost]
        public async Task<IActionResult> EditCourse(EditCourseViewModel editCourse)
        {
            if (ModelState.IsValid && editCourse != null)
            {
                var courseToEdit = await _courseManager.CourseRepository.FindById(editCourse.Id);
                courseToEdit.Name = editCourse.Name;
                courseToEdit.Description = editCourse.Description;
                courseToEdit.AccessLevel = editCourse.AccessLevel;
                _courseManager.CourseRepository.Update(courseToEdit);
                await _courseManager.CourseRepository.SaveChanges();
                _toastNotification.AddSuccessToastMessage("Course was successfully edited");
                return RedirectToAction("CourseConstructor");
            }

            return View(editCourse);
        }

        public async Task<IActionResult> DeleteCourse(Guid id)
        {
            var courseToDelete = await _courseManager.CourseRepository.FindById(id);
            if (courseToDelete != null && !courseToDelete.IsPublished)
            {
                _courseManager.DeleteCourse(courseToDelete);
                await _courseManager.CourseRepository.SaveChanges();
                _toastNotification.AddSuccessToastMessage("Course was successfully deleted");
            }
            else
            {
                _toastNotification.AddErrorToastMessage("You can't delete published course");
            }

            return RedirectToAction("CourseConstructor");
        }

        public async Task<IActionResult> PublishCourse(Guid idCourse)
        {
            var courseToPublish = await _courseManager.CourseRepository.FindByIdWithIncludesAsync(idCourse, new string[] { "Skills", "Materials" });
            if (courseToPublish != null)
            {
                if (_courseManager.PublishCourse(courseToPublish))
                {
                    _courseManager.UpdateCourse(courseToPublish);
                    await _courseManager.CourseRepository.SaveChanges();
                    _toastNotification.AddSuccessToastMessage("Course was successfully published");
                }
                else
                {
                    _toastNotification.AddErrorToastMessage("You can't publish course without any skills or materials");
                    return RedirectToAction("EditCourse", new { id = idCourse });
                }
            }

            return RedirectToAction("CourseConstructor");
        }

        public async Task<IActionResult> Details(Guid id)
        {
            var course = await _courseManager.CourseRepository.FindByIdWithIncludesAsync(id, new string[] { "Skills", "Materials" });
            if (course != null)
            {
                var courseViewModel = new DetailCourseViewModel
                {
                    Id = course.Id,
                    Description = course.Description,
                    AccessLevel = course.AccessLevel,
                    Materials = course.Materials,
                    Skills = course.Skills,
                    Name = course.Name
                };
                return View(courseViewModel);
            }

            return RedirectToAction("Index", "Home");
        }

        public async Task<IActionResult> SubscribeCourse(Guid id)
        {
            var user = await _applicationUserManager.UserRepository.FindByUserName(User.Identity.Name);
            var courseToSubscribe = await _courseManager.CourseRepository.FindByIdWithIncludesAsync(id, new string[] { "Materials", "Skills" });
            if (user != null && courseToSubscribe != null)
            {
                var subscribedCourse = await _courseManager.SubscribeCourse(user, courseToSubscribe);
                await _courseManager.CourseStateManager.CourseStateRepository.SaveChanges();
                if (await _courseManager.CourseStateManager.CheckIfCourseCompleted(user, courseToSubscribe, subscribedCourse))
                {
                    await _courseManager.CourseStateManager.CourseStateRepository.SaveChanges();
                }

                _toastNotification.AddSuccessToastMessage("Successfully subscribed");
            }

            return RedirectToAction("Index", "Home");
        }

        public async Task<IActionResult> UnsubscribeCourse(Guid idCourse, Guid idCourseState)
        {
            var courseState = await _courseManager.CourseStateManager.CourseStateRepository.FindById(idCourseState);
            if (!courseState.IsFinished)
            {
                var courseToUnSubscribe = await _courseManager.CourseRepository.FindById(idCourse);
                if (courseToUnSubscribe != null)
                {
                    _courseManager.UnSubscribeCourse(courseState);
                    await _courseManager.CourseStateManager.CourseStateRepository.SaveChanges();
                }

                _toastNotification.AddSuccessToastMessage("Successfully unsubscribed");
            }
            else
            {
                _toastNotification.AddErrorToastMessage("You can't unsubscribe from completed course");
            }

            return RedirectToAction("MyCourses");
        }
    }
}
