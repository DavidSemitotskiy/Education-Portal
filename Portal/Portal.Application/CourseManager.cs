using Portal.Application.Interfaces;
using Portal.Domain.Interfaces;
using Portal.Domain.Models;

namespace Portal.Application
{
    public class CourseManager : ICourseManager
    {
        public CourseManager(IEntityRepository<Course> repository)
        {
            CourseRepository = repository ?? throw new ArgumentNullException("Repository can't be null");
        }

        public IEntityRepository<Course> CourseRepository { get; }

        public async Task<bool> Exists(string name, string description)
        {
            var allCourses = await CourseRepository.GetAllEntities();
            return allCourses.Any(course => course.Name == name && course.Description == description);
        }

        public void PublishCourse(Course course)
        {
            if (course == null)
            {
                throw new ArgumentNullException("Course can't be null");
            }

            course.IsPublished = true;
        }

        public async Task<IEnumerable<Course>> GetAvailableCourses(User user)
        {
            var allCourses = await CourseRepository.GetAllEntities();
            return allCourses.Where(course => course.AccessLevel <= user.AccessLevel && course.IsPublished);
        }

        public async Task<IEnumerable<Course>> GetOwnCourses(User user)
        {
            var allCourses = await CourseRepository.GetAllEntities();
            return allCourses.Where(course => course.OwnerUser == user.UserId);
        }

        public async Task<IEnumerable<Course>> GetCoursesNotPublished(User user)
        {
            var allCourses = await CourseRepository.GetAllEntities();
            return allCourses.Where(course => course.OwnerUser == user.UserId && !course.IsPublished);
        }

        public async Task AddCourse(Course course)
        {
            if (course == null)
            {
                throw new ArgumentNullException("Course can't be null");
            }

            if (await Exists(course.Name, course.Description))
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
    }
}
