using Microsoft.AspNetCore.Mvc;
using Portal.Application.Interfaces;
using Portal.Domain.Models;
using Portal.WebApp.Models.MaterialViewModels;

namespace Portal.WebApp.Controllers
{
    public class MaterialController : Controller
    {
        private readonly IMaterialManager _materialManager;

        private readonly ICourseManager _courseManager;

        public MaterialController(IMaterialManager materialManager, ICourseManager courseManager)
        {
            _courseManager = courseManager;
            _materialManager = materialManager;
        }

        private async Task<IActionResult> AddMaterial(string viewName, AddMaterialViewModel addMaterialViewModel, Material material, Guid idCourse)
        {
            var courseToUpdate = await _courseManager.CourseRepository.FindByIdWithIncludesAsync(idCourse, new string[] { "Materials" });
            if (courseToUpdate != null)
            {
                if (courseToUpdate.Materials.Contains(material))
                {
                    ModelState.AddModelError(string.Empty, "This material already exists in this course");
                    return View(viewName, addMaterialViewModel);
                }

                courseToUpdate.Materials.Add(material);
                _courseManager.UpdateCourse(courseToUpdate);
                await _courseManager.CourseRepository.SaveChanges();
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
                var material = await _materialManager.CreateOrGetExistedMaterial(new BookMaterial
                {
                    Id = Guid.NewGuid(),
                    Authors = addBookMaterialViewModel.Authors,
                    Title = addBookMaterialViewModel.Title,
                    CountPages = addBookMaterialViewModel.CountPages,
                    DatePublication = addBookMaterialViewModel.DatePublication,
                    Format = addBookMaterialViewModel.Format
                });
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
                var material = await _materialManager.CreateOrGetExistedMaterial(new ArticleMaterial
                {
                    Id = Guid.NewGuid(),
                    Resource = addArticleMaterialViewModel.Resource,
                    DatePublication = addArticleMaterialViewModel.DatePublication
                });
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
                var material = await _materialManager.CreateOrGetExistedMaterial(new VideoMaterial
                {
                    Id = Guid.NewGuid(),
                    Duration = addVideoMaterialViewModel.Duration,
                    Quality = addVideoMaterialViewModel.Quality,
                });
                return await AddMaterial("CreateNewVideo", addVideoMaterialViewModel, material, idCourse);
            }

            return View(addVideoMaterialViewModel);
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
            }

            return RedirectToAction("EditCourse", "Course", new { id = idCourse });
        }
    }
}
