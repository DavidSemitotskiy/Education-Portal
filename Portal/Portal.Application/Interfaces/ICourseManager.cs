using Portal.Domain.Interfaces;
using Portal.Domain.Models;

namespace Portal.Application.Interfaces
{
    public interface ICourseManager
    {
        ICourseRepository CourseRepository { get; set; }

        void AddCourse(Course course);

        void DeleteCourse(Course course);

        IEnumerable<Course> GetAvailableCourses(User user);

        IEnumerable<Course> GetSubscribedCourses(User user);

        IEnumerable<Course> GetOwnCourses(User user);
    }
}
