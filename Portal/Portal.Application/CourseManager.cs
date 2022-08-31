﻿using Portal.Application.Interfaces;
using Portal.Domain.Interfaces;
using Portal.Domain.Models;

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

            if (course.Materials.Count() == 0 || course.Skills.Count() == 0)
            {
                throw new ArgumentException("Course must have one or more materials and skills to be published");
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

        public async Task<IEnumerable<CourseState>> GetCoursesInProgress(User user)
        {
            var allSubscribedCourses = await CourseStateManager.CourseStateRepository.GetAllEntities();
            return allSubscribedCourses.Where(courseState => courseState.UserId == user.UserId);
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

        public Task SubscribeCourse(User user, Course course)
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
    }
}
