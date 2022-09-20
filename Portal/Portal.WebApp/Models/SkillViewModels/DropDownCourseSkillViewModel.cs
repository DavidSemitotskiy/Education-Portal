using Microsoft.AspNetCore.Mvc.Rendering;
using Portal.Domain.Models;

namespace Portal.WebApp.Models.SkillViewModels
{
    public class DropDownCourseSkillViewModel
    {
        public Guid IdCourse { get; set; }

        public Guid SelectedCourseSkillId { get; set; }

        public List<CourseSkill> Skills { get; set; } = new List<CourseSkill>();
    }
}
