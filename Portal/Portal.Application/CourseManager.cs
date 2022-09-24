using Portal.Application.Interfaces;
using Portal.Application.Specifications.CourseSpecifications;
using Portal.Domain.Interfaces;
using Portal.Domain.Models;
using Portal.Domain.Specifications;

namespace Portal.Application
{
    public class CourseManager : ICourseManager
    {
        public CourseManager(IEntityRepository<Course> repository, ICourseStateManager courseStateManager)
        {
            CourseRepository = repository ?? throw new ArgumentNullException("Repository can't be null");
            CourseStateManager = courseStateManager ?? throw new ArgumentNullException("Manager can't be null");
        }

        public IEntityRepository<Course> CourseRepository { get; }

        public ICourseStateManager CourseStateManager { get; }

        public async Task<bool> Exists(Course newCourse)
        {
            var existsSpecification = new ExistsCourseSpecification(newCourse);
            var count = (await CourseRepository.FindEntitiesBySpecification(existsSpecification)).Count();
            return Convert.ToBoolean(count);
        }

        public bool PublishCourse(Course course)
        {
            if (course == null)
            {
                throw new ArgumentNullException("Course can't be null");
            }

            if (course.Materials.Count() == 0 || course.Skills.Count() == 0)
            {
                return false;
            }

            course.IsPublished = true;
            return true;
        }

        public Task<List<Course>> GetAvailableCourses(User user)
        {
            return CourseRepository.FindEntitiesBySpecification(GetAvailableAndPublishedCourseSpecification(user));
        }

        public Task<int> TotalCountOfAvailableCoursesWithSearchString(User user, string searchString)
        {
            var specification = GetAvailableAndPublishedCourseSpecificationWithSearchString(user, searchString);
            return CourseRepository.TotalCountOfEntitiesBySpecification(specification);
        }

        public Task<List<Course>> GetAvailableCoursesByPageWithSearchString(User user, string searchString, int page, int pageSize)
        {
            var specification = GetAvailableAndPublishedCourseSpecificationWithSearchString(user, searchString);
            return CourseRepository.GetEntitiesBySpecificationFromPage(page, pageSize, specification);
        }

        public Task<List<Course>> GetOwnCourses(User user)
        {
            var ownUserCourseSpecification = new OwnUserCourseSpecification(user);
            return CourseRepository.FindEntitiesBySpecification(ownUserCourseSpecification);
        }

        public Task<List<Course>> GetCoursesNotPublished(User user)
        {
            var ownUserCourseSpecification = new OwnUserCourseSpecification(user);
            var notPublishedUserCourseSpecification = new PublishCourseSpecification().Not();
            return CourseRepository.FindEntitiesBySpecification(ownUserCourseSpecification.And(notPublishedUserCourseSpecification));
        }

        public Task<List<CourseState>> GetCoursesInProgress(User user)
        {
            var userCourseInProgressSpecification = new UserCourseInProgressSpecification(user);
            return CourseStateManager.CourseStateRepository.FindEntitiesBySpecification(userCourseInProgressSpecification);
        }

        public async Task AddCourse(Course course)
        {
            if (course == null)
            {
                throw new ArgumentNullException("Course can't be null");
            }

            if (await Exists(course))
            {
                throw new ArgumentException("Course with this name and description already exists");
            }

            await CourseRepository.Add(course);
        }

        public void DeleteCourse(Course course)
        {
           CourseRepository.Delete(course);
        }

        public void UpdateCourse(Course course)
        {
            CourseRepository.Update(course);
        }

        public Task<CourseState> SubscribeCourse(User user, Course course)
        {
            if (user == null || course == null)
            {
                throw new ArgumentNullException("User and course can't be null");
            }

            return CourseStateManager.Subscribe(user, course);
        }

        public void UnSubscribeCourse(CourseState courseState)
        {
            CourseStateManager.UnSubscribe(courseState);
        }

        public async Task<int> CheckIfCoursesCompleted(User user, List<CourseState> courseStates)
        {
            if (courseStates == null)
            {
                throw new ArgumentNullException("CourseState can't be null");
            }

            Course course = null;
            int countChangedRows = 0;
            foreach (var courseState in courseStates)
            {
                course = await CourseRepository.FindByIdWithIncludesAsync(courseState.CourseId, new string[] { "Skills" });
                if (await CourseStateManager.CheckIfCourseCompleted(user, course, courseState))
                {
                    countChangedRows++;
                }
            }

            return countChangedRows;
        }

        public void CompleteMaterial(MaterialState materialState)
        {
            CourseStateManager.CompleteMaterialState(materialState);
        }

        private Specification<Course> GetAvailableAndPublishedCourseSpecification(User user)
        {
            var availableUserCourseSpecification = new AvailableUserCourseSpecification(user);
            var publishCourseSpecification = new PublishCourseSpecification();
            return availableUserCourseSpecification.And(publishCourseSpecification);
        }

        private Specification<Course> GetAvailableAndPublishedCourseSpecificationWithSearchString(User user, string searchString)
        {
            var specification = GetAvailableAndPublishedCourseSpecification(user);
            if (!string.IsNullOrWhiteSpace(searchString))
            {
                var searchCourseBySearchString = new SearchCourseBySearchStringSpecification(searchString);
                specification = specification.And(searchCourseBySearchString);
            }

            return specification;
        }
    }
}
