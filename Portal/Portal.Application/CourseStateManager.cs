using Portal.Application.Interfaces;
using Portal.Domain.Interfaces;
using Portal.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Portal.Application
{
    public class CourseStateManager : ICourseStateManager
    {
        public CourseStateManager(IEntityRepository<CourseState> courseStateRepository, IMaterialStateManager materialStateManager, IUserSkillManager userSkillManager, IUserManager userManager)
        {
            CourseStateRepository = courseStateRepository ?? throw new ArgumentNullException("Repository can't be null");
            MaterialStateManager = materialStateManager ?? throw new ArgumentNullException("Manager can't be null");
            UserSkillManager = userSkillManager ?? throw new ArgumentNullException("Manager can't be null");
            UserManager = userManager ?? throw new ArgumentNullException("Manager can't be null");
        }

        public IEntityRepository<CourseState> CourseStateRepository { get; }

        public IMaterialStateManager MaterialStateManager { get; }

        public IUserManager UserManager { get; }

        public IUserSkillManager UserSkillManager { get; }

        public async Task Subscribe(User user, Course course)
        {
            if (user == null || course == null)
            {
                throw new ArgumentNullException("User and course can't be null");
            }

            var courseState = new CourseState
            {
                Id = Guid.NewGuid(),
                CourseId = course.Id,
                UserId = user.UserId,
                IsFinished = false,
            };
            if (await Exists(user, courseState))
            {
                throw new ArgumentNullException("You already subscribed on this course");
            }

            courseState.MaterialStates = await MaterialStateManager.GetMaterialStatesFromCourse(user, course);
            CourseStateRepository.Add(courseState);
        }

        public void UnSubscribe(CourseState courseState)
        {
            if (courseState == null)
            {
                throw new ArgumentNullException("CourseState can't be null");
            }

            CourseStateRepository.Delete(courseState);
        }

        public async Task<bool> CheckIfCourseCompleted(User user, Course course, CourseState courseState)
        {
            if (courseState == null || user == null || course == null)
            {
                throw new ArgumentNullException("CourseState, User and Course can't be null");
            }

            var userWithIncludes = await UserManager.UserRepository.FindByIdWithIncludesAsync(user.UserId, new string[] { "Skills" });
            var courseStateWithIncludes = await CourseStateRepository.FindByIdWithIncludesAsync(courseState.Id, new string[] { "MaterialStates" });
            if (courseStateWithIncludes.MaterialStates.All(materialState => materialState.IsCompleted)
                && courseStateWithIncludes.IsFinished != true)
            {
                foreach (var courseSkill in course.Skills)
                {
                    await UserSkillManager.AddUserSkill(userWithIncludes, courseSkill);
                }

                courseStateWithIncludes.IsFinished = true;
                CourseStateRepository.Update(courseStateWithIncludes);
                return true;
            }

            return false;
        }

        public void CompleteMaterialState(MaterialState materialState)
        {
            MaterialStateManager.CompleteMaterial(materialState);
        }

        public async Task<bool> Exists(User user, CourseState courseState)
        {
            var allMaterials = await CourseStateRepository.GetAllEntities();
            return allMaterials.Any(state => state.CourseId == courseState.CourseId && state.UserId == courseState.UserId);
        }

        public async Task<string> GetCourseProgress(CourseState courseState)
        {
            if (courseState == null)
            {
                throw new ArgumentNullException("CourseState can't be null");
            }

            var courseStateWithIncludes = await CourseStateRepository.FindByIdWithIncludesAsync(courseState.Id, new string[] { "MaterialStates" });
            var countFinishedMaterials = courseStateWithIncludes.MaterialStates.Count(materialState => materialState.IsCompleted);
            var countMaterials = courseStateWithIncludes.MaterialStates.Count();
            return $"{countFinishedMaterials}/{countMaterials}";
        }
    }
}
