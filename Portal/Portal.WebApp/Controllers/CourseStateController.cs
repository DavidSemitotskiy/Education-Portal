using Microsoft.AspNetCore.Mvc;
using Portal.Application.Interfaces;
using Portal.WebApp.Models.CourseViewModels;

namespace Portal.WebApp.Controllers
{
    public class CourseStateController : Controller
    {
        private readonly ICourseManager _courseManager;

        private readonly IMaterialManager _materialManager;

        private readonly IMaterialStateManager _materialStateManager;

        private readonly IApplicationUserManager _userManager;

        public CourseStateController(ICourseManager courseManager, IMaterialManager materialManager, IMaterialStateManager materialStateManager, IApplicationUserManager userManager)
        {
            _courseManager = courseManager;
            _materialManager = materialManager;
            _materialStateManager = materialStateManager;
            _userManager = userManager;
        }

        public async Task<IActionResult> CompleteMaterial(Guid idCourseState, Guid idMaterialState)
        {
            var materialState = await _materialStateManager.MaterialStateRepository.FindById(idMaterialState);
            var user = await _userManager.UserRepository.FindByUserName(User.Identity.Name);
            if (materialState != null)
            {
                _materialStateManager.CompleteMaterial(materialState);
                var coursesInProgress = await _courseManager.GetCoursesInProgress(user);
                await _courseManager.CheckIfCoursesCompleted(user, coursesInProgress);
                await _courseManager.CourseStateManager.MaterialStateManager.
                    MaterialStateRepository.SaveChanges();
            }

            return RedirectToAction("Details", new { idCourseState });
        }

        public async Task<IActionResult> Details(Guid idCourseState)
        {
            var courseState = await _courseManager.CourseStateManager.CourseStateRepository.FindByIdWithIncludesAsync(idCourseState, new string[] { "MaterialStates" });
            List<CompleteCourseViewModel> list = new List<CompleteCourseViewModel>();
            foreach (var materialState in courseState.MaterialStates)
            {
                list.Add(new CompleteCourseViewModel
                {
                    MaterialName = (await _materialManager.MaterialRepository.FindById(materialState.OwnerMaterial)).ToString(),
                    MaterialState = materialState,
                    CourseStateId = idCourseState
                });
            }
            return View(list);
        }
    }
}
