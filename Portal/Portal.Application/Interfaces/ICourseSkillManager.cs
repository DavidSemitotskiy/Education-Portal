using Portal.Domain.Interfaces;
using Portal.Domain.Models;

namespace Portal.Application.Interfaces
{
    public interface ICourseSkillManager
    {
        IEntityRepository<CourseSkill> CourseSkillRepository { get; }

        Task<CourseSkill> CreateOrGetExistedCourseSkill(CourseSkill skill);

        void DeleteCourseSkill(Course course, CourseSkill skill);

        void UpdateCourseSkill(Course course, int indexCourseSkill, CourseSkill newSkill);
    }
}
