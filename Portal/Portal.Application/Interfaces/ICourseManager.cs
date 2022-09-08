using Portal.Domain.Interfaces;
using Portal.Domain.Models;

namespace Portal.Application.Interfaces
{
    public interface ICourseManager
    {
        IEntityRepository<Course> CourseRepository { get; }

        ICourseStateManager CourseStateManager { get; }

        Task AddCourse(Course course);

        void DeleteCourse(Course course);

        void UpdateCourse(Course course);

        Task<bool> Exists(string name, string description);

        void PublishCourse(Course course);

        Task<int> CheckIfCoursesCompleted(User user, List<CourseState> courseStates);

        void CompleteMaterial(MaterialState materialState);

        Task<List<Course>> GetAvailableCourses(User user);

        Task<IEnumerable<Course>> GetOwnCourses(User user);

        Task<IEnumerable<Course>> GetCoursesNotPublished(User user);

        Task<IEnumerable<CourseState>> GetCoursesInProgress(User user);

        Task<CourseState> SubscribeCourse(User user, Course course);

        void UnSubscribeCourse(CourseState courseState);
    }
}
