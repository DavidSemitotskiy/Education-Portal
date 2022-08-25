﻿using Portal.Domain.Interfaces;
using Portal.Domain.Models;

namespace Portal.Application.Interfaces
{
    public interface ICourseManager
    {
        IEntityRepository<Course> CourseRepository { get; }

        Task AddCourse(Course course);

        void DeleteCourse(Course course);

        void UpdateCourse(Course course);

        Task<bool> Exists(string name, string description);

        Task<IEnumerable<Course>> GetAvailableCourses(User user);

        Task<IEnumerable<Course>> GetOwnCourses(User user);
    }
}
