using AutoMapper;
using Portal.Domain.Models;
using Portal.WebApp.Models.SkillViewModels;

namespace Portal.WebApp.Helpers
{
    public class SkillMappings : Profile
    {
        public SkillMappings()
        {
            CreateMap<AddCourseSkillViewModel, CourseSkill>().AfterMap((src, dest) =>
            {
                dest.Id = Guid.NewGuid();
            });
        }
    }
}
