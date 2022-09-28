﻿using Microsoft.AspNetCore.Mvc;
using NToastNotify;
using Portal.Application.Interfaces;
using Portal.Domain.Models;
using Portal.WebApp.Models.SkillViewModels;
using Portal.WebApp.Extensions;
using Portal.WebApp.Filters;
using Portal.WebApp.Resources;
using AutoMapper;

namespace Portal.WebApp.Controllers
{
    public class CourseSkillController : Controller
    {
        private readonly ICourseSkillManager _courseSkillManager;

        private readonly ICourseManager _courseManager;

        private readonly IToastNotification _toastNotification;

        private readonly IMapper _mapper;

        public CourseSkillController(ICourseSkillManager courseSkillManager, ICourseManager courseManager, IToastNotification toastNotification, IMapper mapper)
        {
            _courseSkillManager = courseSkillManager;
            _courseManager = courseManager;
            _toastNotification = toastNotification;
            _mapper = mapper;
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
                var skill = _mapper.Map<CourseSkill>(addCourseSkillViewModel);
                var courseSkillToAdd = await _courseSkillManager.CreateOrGetExistedCourseSkill(skill);
                if (courseToEdit != null)
                {
                    if (courseToEdit.Skills.Contains(courseSkillToAdd))
                    {
                        ModelState.AddModelError("CourseSkill", ValidationErrorMessages.SkillAlreadyExistsInCourse);
                        return View(addCourseSkillViewModel);
                    }

                    courseToEdit.Skills.Add(courseSkillToAdd);
                    _courseManager.UpdateCourse(courseToEdit);
                    await _courseManager.CourseRepository.SaveChanges();
                    _toastNotification.AddSuccessToastMessage(NotificationsMessages.NewSkillAddedToCourse);
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
                _toastNotification.AddErrorToastMessage(NotificationsMessages.NoExistingCourseSkillExists);
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
                    _toastNotification.AddErrorToastMessage(ValidationErrorMessages.SkillAlreadyExistsInCourse);
                    return RedirectToAction("AddExisting", new { id = idCourse });
                }

                course.Skills.Add(courseSkillToAdd);
                _courseManager.UpdateCourse(course);
                await _courseManager.CourseRepository.SaveChanges();
                _toastNotification.AddSuccessToastMessage(NotificationsMessages.CourseSkillAddedToCourse);
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
                _toastNotification.AddSuccessToastMessage(NotificationsMessages.CourseSkillDeletedFromCourse);
            }

            return RedirectToAction("EditCourse", "Course", new { id = idCourse });
        }
    }
}
