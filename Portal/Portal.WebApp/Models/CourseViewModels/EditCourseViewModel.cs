using Portal.Domain.Models;
using System.ComponentModel.DataAnnotations;

namespace Portal.WebApp.Models.CourseViewModels
{
    public class EditCourseViewModel
    {
        public Guid Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string Description { get; set; }

        public int AccessLevel { get; set; }

        public List<Material> Materials { get; set; }

        public List<CourseSkill> Skills { get; set; }

        public bool IsPublished { get; set; }
    }
}
