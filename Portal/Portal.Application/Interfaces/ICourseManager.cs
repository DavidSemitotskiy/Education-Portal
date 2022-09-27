﻿using Portal.Domain.Interfaces;
using Portal.Domain.Models;

namespace Portal.Application.Interfaces
{
    public interface ICourseManager
    {
        IEntityRepository<Course> CourseRepository { get; }

        ICourseStateManager CourseStateManager { get; }

        Task<bool> AddCourse(Course course);

        void DeleteCourse(Course course);

        void UpdateCourse(Course course);

        Task<bool> Exists(Course newCourse);

        bool PublishCourse(Course course);

        Task<int> CheckIfCoursesCompleted(User user, List<CourseState> courseStates);

        void CompleteMaterial(MaterialState materialState);

        Task<List<Course>> GetAvailableCourses(User user);

        Task<int> TotalCountOfAvailableCoursesWithSearchString(User user, string searchString);

        Task<int> TotalCountOfOwnCourses(User user);

        Task<List<Course>> GetAvailableCoursesByPageWithSearchString(User user, string searchString, int page, int pageSize);

        Task<List<Course>> GetOwnCourses(User user);

        Task<List<Course>> GetOwnCoursesByPage(User user, int page, int pageSize);

        Task<List<Course>> GetCoursesNotPublished(User user);

        Task<List<CourseState>> GetCoursesInProgress(User user);

        Task<CourseState> SubscribeCourse(User user, Course course);

        void UnSubscribeCourse(CourseState courseState);
    }
}
