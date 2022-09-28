using Microsoft.AspNetCore.Mvc;
using NToastNotify;
using Portal.Application.Interfaces;
using Portal.Domain.Models;
using Portal.WebApp.Models.MaterialViewModels;
using Portal.WebApp.Extensions;
using Portal.Application.Validation;
using Portal.WebApp.Filters;
using Portal.WebApp.Resources;
using AutoMapper;

namespace Portal.WebApp.Controllers
{
    public class MaterialController : Controller
    {
        private readonly IMaterialManager _materialManager;

        private readonly ICourseManager _courseManager;

        private readonly IToastNotification _toastNotification;

        private readonly IMapper _mapper;

        public MaterialController(IMaterialManager materialManager, ICourseManager courseManager, IToastNotification toastNotification, IMapper mapper)
        {
            _courseManager = courseManager;
            _materialManager = materialManager;
            _toastNotification = toastNotification;
            _mapper = mapper;
        }

        private async Task<IActionResult> AddMaterial(string viewName, AddMaterialViewModel addMaterialViewModel, Material material, Guid idCourse)
        {
            var courseToUpdate = await _courseManager.CourseRepository.FindByIdWithIncludesAsync(idCourse, new string[] { "Materials" });
            if (courseToUpdate != null)
            {
                if (courseToUpdate.Materials.Contains(material))
                {
                    ModelState.AddModelError(string.Empty, ValidationErrorMessages.MaterialAlreadyExists);
                    return View(viewName, addMaterialViewModel);
                }

                courseToUpdate.Materials.Add(material);
                _courseManager.UpdateCourse(courseToUpdate);
                await _courseManager.CourseRepository.SaveChanges();
                _toastNotification.AddSuccessToastMessage(NotificationsMessages.NewMaterialAddedToCourse);
                return RedirectToAction("EditCourse", "Course", new { id = idCourse });
            }

            return View(viewName, addMaterialViewModel);
        }

        public IActionResult CreateNewBook(Guid id)
        {
            var bookMaterialViewModel = new AddBookMaterialViewModel
            {
                IdCourse = id
            };

            return View(bookMaterialViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> CreateNewBook(AddBookMaterialViewModel addBookMaterialViewModel, Guid idCourse)
        {
            if (ModelState.IsValid && addBookMaterialViewModel != null)
            {
                var book = _mapper.Map<BookMaterial>(addBookMaterialViewModel);
                var material = await _materialManager.CreateOrGetExistedMaterial(book);
                var errorMessages = new ErrorMessages<BookMaterialValidator, BookMaterial>();
                if (!await errorMessages.ValidateModel((BookMaterial)material, ModelState))
                {
                    return View(addBookMaterialViewModel);
                }

                return await AddMaterial("CreateNewBook", addBookMaterialViewModel, material, idCourse);
            }

            return View(addBookMaterialViewModel);
        }

        public IActionResult CreateNewArticle(Guid id)
        {
            var articleMaterialViewModel = new AddArticleMaterialViewModel
            {
                IdCourse = id
            };

            return View(articleMaterialViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> CreateNewArticle(AddArticleMaterialViewModel addArticleMaterialViewModel, Guid idCourse)
        {
            if (ModelState.IsValid && addArticleMaterialViewModel != null)
            {
                var article = _mapper.Map<ArticleMaterial>(addArticleMaterialViewModel);
                var material = await _materialManager.CreateOrGetExistedMaterial(article);
                return await AddMaterial("CreateNewArticle", addArticleMaterialViewModel, material, idCourse);
            }

            return View(addArticleMaterialViewModel);
        }

        public IActionResult CreateNewVideo(Guid id)
        {
            var videoMaterialViewModel = new AddVideoMaterialViewModel
            {
                IdCourse = id
            };

            return View(videoMaterialViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> CreateNewVideo(AddVideoMaterialViewModel addVideoMaterialViewModel, Guid idCourse)
        {
            if (ModelState.IsValid && addVideoMaterialViewModel != null)
            {
                var video = _mapper.Map<VideoMaterial>(addVideoMaterialViewModel);
                var material = await _materialManager.CreateOrGetExistedMaterial(video);
                var errorMessages = new ErrorMessages<VideoMaterialValidator, VideoMaterial>();
                if (!await errorMessages.ValidateModel((VideoMaterial)material, ModelState))
                {
                    return View(addVideoMaterialViewModel);
                }

                return await AddMaterial("CreateNewVideo", addVideoMaterialViewModel, material, idCourse);
            }

            return View(addVideoMaterialViewModel);
        }

        [TypeFilter(typeof(PaginationFilterAttribute))]
        public async Task<IActionResult> AddExisting(string filterString, Guid id, int pageNumber, int pageSize)
        {
            var materialsByPage = await _materialManager.GetMaterialsByPageWithFilterString(filterString, pageNumber, pageSize);
            if (materialsByPage.Count() == 0)
            {
                _toastNotification.AddErrorToastMessage(NotificationsMessages.NoExistingMaterialExists);
                return RedirectToAction("EditCourse", "Course", new { id });
            }

            var existingMaterialViewModels = materialsByPage.Select(material => new AddExistingMaterialViewModel
            {
                IdCourse = id,
                Id = material.Id,
                Material = material.ToString()
            }).ToList();
            var page = this.GetPage(existingMaterialViewModels, await _materialManager.TotalCountOfMaterialsWithFilterString(filterString), pageNumber, pageSize);
            return View(page);
        }

        public async Task<IActionResult> AddExistingMaterial(Guid idCourse, Guid idMaterial)
        {
            var course = await _courseManager.CourseRepository.FindByIdWithIncludesAsync(idCourse, new string[] { "Materials" });
            var material = await _materialManager.MaterialRepository.FindById(idMaterial);
            if (course != null && material != null)
            {
                if (course.Materials.Contains(material))
                {
                    _toastNotification.AddErrorToastMessage(ValidationErrorMessages.MaterialAlreadyExists);
                    return RedirectToAction("AddExisting", new { id = idCourse });
                }

                course.Materials.Add(material);
                _courseManager.UpdateCourse(course);
                await _courseManager.CourseRepository.SaveChanges();
                _toastNotification.AddSuccessToastMessage(NotificationsMessages.MaterialAddedToCourse);
            }

            return RedirectToAction("EditCourse", "Course", new { id = idCourse });
        }

        public async Task<IActionResult> DeleteMaterialFromCourse(Guid idCourse, Guid idMaterial)
        {
            var course = await _courseManager.CourseRepository.FindByIdWithIncludesAsync(idCourse, new string[] { "Materials" });
            var materialToDelete = await _materialManager.MaterialRepository.FindById(idMaterial);
            if (course != null && materialToDelete != null)
            {
                course.Materials.Remove(materialToDelete);
                _courseManager.UpdateCourse(course);
                await _courseManager.CourseRepository.SaveChanges();
                _toastNotification.AddSuccessToastMessage(NotificationsMessages.MaterialDeletedFromCourse);
            }

            return RedirectToAction("EditCourse", "Course", new { id = idCourse });
        }
    }
}
