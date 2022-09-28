using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NToastNotify;
using Portal.Application.Interfaces;
using Portal.Application.Validation;
using Portal.Domain.Models;
using Portal.WebApp.Extensions;
using Portal.WebApp.Filters;
using Portal.WebApp.Models.CourseViewModels;
using Portal.WebApp.Resources;

namespace Portal.WebApp.Controllers
{
    [Authorize]
    public class CourseController : Controller
    {
        private readonly ICourseManager _courseManager;

        private readonly IApplicationUserManager _applicationUserManager;

        private IToastNotification _toastNotification;

        private readonly IMapper _mapper;

        public CourseController(ICourseManager courseManager, IApplicationUserManager applicationUserManager, IToastNotification toastNotification, IMapper mapper)
        {
            _courseManager = courseManager;
            _applicationUserManager = applicationUserManager;
            _toastNotification = toastNotification;
            _mapper = mapper;
        }

        [TypeFilter(typeof(PaginationFilterAttribute))]
        public async Task<IActionResult> CourseConstructor(int pageNumber, int pageSize)
        {
            var user = await _applicationUserManager.UserRepository.FindByUserName(User.Identity.Name);
            var ownCourses = await _courseManager.GetOwnCoursesByPage(user, pageNumber, pageSize);
            var courseViewModels = ownCourses.Select(course => new CourseViewModel
            {
                Id = course.Id,
                Name = course.Name,
                Description = course.Description,
                IsPublished = course.IsPublished
            }).ToList();
            var page = this.GetPage(courseViewModels, await _courseManager.TotalCountOfOwnCourses(user), pageNumber, pageSize);
            return View(page);
        }

        [TypeFilter(typeof(PaginationFilterAttribute))]
        public async Task<IActionResult> MyCourses(int pageNumber, int pageSize)
        {
            var user = await _applicationUserManager.UserRepository.FindByUserName(User.Identity.Name);
            var coursesInProgress = await _courseManager.GetCoursesInProgressByPage(user, pageNumber, pageSize);
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

            var page = this.GetPage(courseStateViewModels, await _courseManager.TotalCountOfCoursesInProgress(user), pageNumber, pageSize);
            return View(page);
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
                var errorMessages = new ErrorMessages<CourseValidator, Course>();
                if (!await errorMessages.ValidateModel(courseToCreate, ModelState))
                {
                    return View(createdCourse);
                }

                if (await _courseManager.AddCourse(courseToCreate))
                {
                    await _courseManager.CourseRepository.SaveChanges();
                    _toastNotification.AddSuccessToastMessage(NotificationsMessages.CourseCreated);
                    return RedirectToAction("CourseConstructor");
                }

                _toastNotification.AddErrorToastMessage(NotificationsMessages.CourseAlreadyExists);
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
                var errorMessages = new ErrorMessages<CourseValidator, Course>();
                if (!await errorMessages.ValidateModel(courseToEdit, ModelState))
                {
                    _toastNotification.AddErrorToastMessage(NotificationsMessages.IncorrectInputData);
                    return RedirectToAction("EditCourse", new { id = editCourse.Id });
                }

                if (await _courseManager.Exists(courseToEdit))
                {
                    _toastNotification.AddErrorToastMessage(NotificationsMessages.CourseAlreadyExists);
                    return RedirectToAction("EditCourse", new { id = editCourse.Id });
                }

                _courseManager.CourseRepository.Update(courseToEdit);
                await _courseManager.CourseRepository.SaveChanges();
                _toastNotification.AddSuccessToastMessage(NotificationsMessages.CourseEdited);
                return RedirectToAction("CourseConstructor");
            }

            _toastNotification.AddErrorToastMessage(NotificationsMessages.IncorrectInputData);
            return RedirectToAction("EditCourse", new { id = editCourse?.Id });
        }

        public async Task<IActionResult> DeleteCourse(Guid id)
        {
            var courseToDelete = await _courseManager.CourseRepository.FindById(id);
            if (courseToDelete != null && !courseToDelete.IsPublished)
            {
                _courseManager.DeleteCourse(courseToDelete);
                await _courseManager.CourseRepository.SaveChanges();
                _toastNotification.AddSuccessToastMessage(NotificationsMessages.CourseDeleted);
            }
            else
            {
                _toastNotification.AddErrorToastMessage(NotificationsMessages.CantDeletePublishedCourse);
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
                    _toastNotification.AddSuccessToastMessage(NotificationsMessages.CoursePublished);
                }
                else
                {
                    _toastNotification.AddErrorToastMessage(NotificationsMessages.CantPublishCourse);
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
                var courseViewModel = _mapper.Map<DetailCourseViewModel>(course);
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
                if (subscribedCourse == null)
                {
                    _toastNotification.AddErrorToastMessage(NotificationsMessages.AlreadySubscribed);
                    return RedirectToAction("Index", "Home");
                }

                await _courseManager.CourseStateManager.CourseStateRepository.SaveChanges();
                if (await _courseManager.CourseStateManager.CheckIfCourseCompleted(user, courseToSubscribe, subscribedCourse))
                {
                    await _courseManager.CourseStateManager.CourseStateRepository.SaveChanges();
                }

                _toastNotification.AddSuccessToastMessage(NotificationsMessages.SuccessfullySubscribed);
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

                _toastNotification.AddSuccessToastMessage(NotificationsMessages.SuccessfullyUnSubscribed);
            }
            else
            {
                _toastNotification.AddErrorToastMessage(NotificationsMessages.CantUnSubscribe);
            }

            return RedirectToAction("MyCourses");
        }
    }
}
