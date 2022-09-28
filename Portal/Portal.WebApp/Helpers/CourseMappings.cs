using AutoMapper;
using Portal.Domain.Models;
using Portal.WebApp.Models.CourseViewModels;

namespace Portal.WebApp.Helpers
{
    public class CourseMappings : Profile
    {
        public CourseMappings()
        {
            CreateMap<Course, DetailCourseViewModel>();
            CreateMap<Course, EditCourseViewModel>();
            CreateMap<CreateCourseViewModel, Course>().AfterMap((src, dest) =>
            {
                dest.Id = Guid.NewGuid();
                dest.IsPublished = false;
            });
        }
    }
}
