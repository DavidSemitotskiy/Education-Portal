using Portal.Domain.Models;

namespace Portal.WebApp.Models.CourseViewModels
{
    public class DetailCourseViewModel
    {
        public Guid Id { get; set; }

        public string Description { get; set; }

        public string Name { get; set; }

        public int AccessLevel { get; set; }

        public List<Material> Materials { get; set; }

        public List<CourseSkill> Skills { get; set; } 
    }
}
