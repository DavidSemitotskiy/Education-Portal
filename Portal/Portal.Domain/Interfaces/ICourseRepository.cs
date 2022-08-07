using Portal.Domain.Models;

namespace Portal.Domain.Interfaces
{
    public interface ICourseRepository
    {
        List<Course> Courses { get; set; }

        IEnumerable<Course> GetAllCourses();

        void Add(Course course);

        void Delete(Course course);

        bool Exists(string name, string description);

        void SaveChanges();
    }
}
