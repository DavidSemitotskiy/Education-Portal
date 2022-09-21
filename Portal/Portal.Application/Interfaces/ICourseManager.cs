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

        Task<bool> Exists(Course newCourse);

        void PublishCourse(Course course);

        Task<int> CheckIfCoursesCompleted(User user, List<CourseState> courseStates);

        void CompleteMaterial(MaterialState materialState);

        Task<List<Course>> GetAvailableCourses(User user);

        Task<int> TotalCountOfAvailableCourses(User user);

        Task<List<Course>> GetAvailableCoursesByPage(User user, int page, int pageSize);

        Task<List<Course>> GetOwnCourses(User user);

        Task<List<Course>> GetCoursesNotPublished(User user);

        Task<List<CourseState>> GetCoursesInProgress(User user);

        Task<CourseState> SubscribeCourse(User user, Course course);

        void UnSubscribeCourse(CourseState courseState);
    }
}
