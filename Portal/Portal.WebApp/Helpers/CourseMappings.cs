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
        }
    }
}
