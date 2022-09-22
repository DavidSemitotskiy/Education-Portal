using Portal.Domain.Models;

namespace Portal.WebApp.Models.SkillViewModels
{
    public class AddExistingCourseSkillViewModel
    {
        public Guid IdCourse { get; set; }

        public List<CourseSkill> Skills { get; set; }
    }
}
