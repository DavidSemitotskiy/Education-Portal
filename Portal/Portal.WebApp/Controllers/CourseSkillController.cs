using Microsoft.AspNetCore.Mvc;
using NToastNotify;
using Portal.Application.Interfaces;
using Portal.Domain.Models;
using Portal.WebApp.Models.SkillViewModels;
using Portal.WebApp.Extensions;
using Portal.WebApp.Filters;

namespace Portal.WebApp.Controllers
{
    public class CourseSkillController : Controller
    {
        private readonly ICourseSkillManager _courseSkillManager;

        private readonly ICourseManager _courseManager;

        private readonly IToastNotification _toastNotification;

        public CourseSkillController(ICourseSkillManager courseSkillManager, ICourseManager courseManager, IToastNotification toastNotification)
        {
            _courseSkillManager = courseSkillManager;
            _courseManager = courseManager;
            _toastNotification = toastNotification;
        }

        public IActionResult CreateNew(Guid id)
        {
            var addCourseSkillViewModel = new AddCourseSkillViewModel
            {
                IdCourse = id
            };
            return View(addCourseSkillViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> CreateNew(AddCourseSkillViewModel addCourseSkillViewModel)
        {
            if (ModelState.IsValid && addCourseSkillViewModel != null)
            {
                var courseToEdit = await _courseManager.CourseRepository.FindByIdWithIncludesAsync(addCourseSkillViewModel.IdCourse, new string[] { "Skills" });
                var courseSkillToAdd = await _courseSkillManager.CreateOrGetExistedCourseSkill(new CourseSkill
                {
                    Id = Guid.NewGuid(),
                    Experience = addCourseSkillViewModel.CourseSkill
                });
                if (courseToEdit != null)
                {
                    if (courseToEdit.Skills.Contains(courseSkillToAdd))
                    {
                        ModelState.AddModelError("CourseSkill", "This Skill already exists in this course!");
                        return View(addCourseSkillViewModel);
                    }

                    courseToEdit.Skills.Add(courseSkillToAdd);
                    _courseManager.UpdateCourse(courseToEdit);
                    await _courseManager.CourseRepository.SaveChanges();
                    _toastNotification.AddSuccessToastMessage("Successfully added new skill to course");
                    return RedirectToAction("EditCourse", "Course", new { id = addCourseSkillViewModel.IdCourse });
                }
            }

            return View(addCourseSkillViewModel);
        }

        [TypeFilter(typeof(PaginationFilterAttribute))]
        public async Task<IActionResult> AddExisting(Guid id, int pageNumber, int pageSize)
        {
            var skillsByPage = await _courseSkillManager.CourseSkillRepository.GetEntitiesFromPage(pageNumber, pageSize);
            if (skillsByPage.Count() == 0)
            {
                _toastNotification.AddErrorToastMessage("There aren't any existing skills");
                return RedirectToAction("EditCourse", "Course", new { id });
            }

            var existingCourseSkillViewModels = skillsByPage.Select(skill => new AddExistingCourseSkillViewModel
            {
                IdCourse = id,
                Id = skill.Id,
                Skill = skill.Experience
            }).ToList();
            var page = this.GetPage(existingCourseSkillViewModels, await _courseSkillManager.CourseSkillRepository.TotalCountOfEntities(), pageNumber, pageSize);
            return View(page);
        }

        public async Task<IActionResult> AddExistingSkill(Guid idCourse, Guid idSkill)
        {
            var course = await _courseManager.CourseRepository.FindByIdWithIncludesAsync(idCourse, new string[] { "Skills" });
            var courseSkillToAdd = await _courseSkillManager.CourseSkillRepository.FindById(idSkill);
            if (courseSkillToAdd != null && course != null)
            {
                if (course.Skills.Contains(courseSkillToAdd))
                {
                    _toastNotification.AddErrorToastMessage("This Skill already exists in this course");
                    return RedirectToAction("AddExisting", new { id = idCourse });
                }

                course.Skills.Add(courseSkillToAdd);
                _courseManager.UpdateCourse(course);
                await _courseManager.CourseRepository.SaveChanges();
                _toastNotification.AddSuccessToastMessage("Successfully added existing skill");
            }

            return RedirectToAction("EditCourse", "Course", new { id = idCourse });
        }

        public async Task<IActionResult> DeleteSkillFromCourse(Guid idCourse, Guid idSkill)
        {
            var skill = await _courseSkillManager.CourseSkillRepository.FindById(idSkill);
            var course = await _courseManager.CourseRepository.FindByIdWithIncludesAsync(idCourse, new string[] { "Skills" });
            if (skill != null && course != null)
            {
                _courseSkillManager.DeleteCourseSkill(course, skill);
                _courseManager.CourseRepository.Update(course);
                await _courseManager.CourseRepository.SaveChanges();
                _toastNotification.AddSuccessToastMessage("Successfully deleted skill from course");
            }

            return RedirectToAction("EditCourse", "Course", new { id = idCourse });
        }
    }
}
