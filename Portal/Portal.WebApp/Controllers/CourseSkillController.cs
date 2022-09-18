using Microsoft.AspNetCore.Mvc;
using Portal.Application.Interfaces;
using Portal.Domain.Models;
using Portal.WebApp.Models.SkillViewModels;

namespace Portal.WebApp.Controllers
{
    public class CourseSkillController : Controller
    {
        private readonly ICourseSkillManager _courseSkillManager;

        private readonly ICourseManager _courseManager;

        public CourseSkillController(ICourseSkillManager courseSkillManager, ICourseManager courseManager)
        {
            _courseSkillManager = courseSkillManager;
            _courseManager = courseManager;
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
                    return RedirectToAction("EditCourse", "Course", new { id = addCourseSkillViewModel.IdCourse });
                }
            }

            return View(addCourseSkillViewModel);
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
            }

            return RedirectToAction("EditCourse", "Course", new { id = idCourse });
        }
    }
}
