using System.ComponentModel.DataAnnotations;

namespace Portal.WebApp.Models.SkillViewModels
{
    public class AddCourseSkillViewModel
    {
        public Guid IdCourse { get; set; }

        [Required]
        public string Experience { get; set; }
    }
}
