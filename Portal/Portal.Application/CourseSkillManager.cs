using Portal.Application.Interfaces;
using Portal.Domain.Interfaces;
using Portal.Domain.Models;

namespace Portal.Application
{
    public class CourseSkillManager : ICourseSkillManager
    {
        public CourseSkillManager(IEntityRepository<CourseSkill> repository)
        {
            CourseSkillRepository = repository ?? throw new ArgumentNullException("Repository can't be null");
        }

        public IEntityRepository<CourseSkill> CourseSkillRepository { get; }

        public async Task<CourseSkill> CreateOrGetExistedCourseSkill(CourseSkill skill)
        {
            var allCourseSkills = await CourseSkillRepository.GetAllEntities();
            var certainSkill = allCourseSkills.FirstOrDefault(s => s.Experience == skill.Experience);
            if (certainSkill == null)
            {
                await CourseSkillRepository.Add(skill);
                return skill;
            }

            return certainSkill;
        }

        public void DeleteCourseSkill(Course course, CourseSkill skill)
        {
            if (course == null)
            {
                throw new ArgumentNullException("Course can't be null");
            }

            var certainSkill = course.Skills.FirstOrDefault(s => s.Experience == skill.Experience);
            course.Skills.Remove(certainSkill);
        }

        public void UpdateCourseSkill(Course course, int indexCourseSkill, CourseSkill newSkill)
        {
            if (course == null)
            {
                throw new ArgumentNullException("Course can't be null");
            }

            course.Skills[indexCourseSkill] = newSkill;
        }
    }
}
