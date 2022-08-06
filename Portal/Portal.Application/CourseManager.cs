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
    internal class CourseManager : ICourseManager
    {
        public CourseManager(ICourseRepository courseRepository)
        {
            CourseRepository = courseRepository ?? throw new ArgumentNullException("Repository can't be null");
        }

        public ICourseRepository CourseRepository { get; set; }

        public void AddCourse(Course course)
        {
            if (course == null)
            {
                throw new ArgumentNullException("Course can't be null");
            }

            if (CourseRepository.Exists(course.Name, course.Description))
            {
                throw new ArgumentException("Course with this name and description already exists");
            }

            CourseRepository.Add(course);
            //CourseRepository.SaveChanges();
        }

        public void DeleteCourse(Course course)
        {
            CourseRepository.Delete(course);
            CourseRepository.SaveChanges();
        }

        public IEnumerable<Course> GetAvailableCourses(User user)
        {
            return CourseRepository.GetAllCourses().Where(course => course.AccessLevel <= user.AccessLevel);
        }

        public IEnumerable<Course> GetSubscribedCourses(User user)
        {
            return new List<Course>();
        }

        public IEnumerable<Course> GetOwnCourses(User user)
        {
            return CourseRepository.GetAllCourses().Where(course => course.Owner.IdUser == user.IdUser);
        }
    }
}
