using Portal.Domain.BaseModels;

namespace Portal.Domain.Models
{
    public class Course : Entity
    {
        public string Name { get; set; }

        public string Description { get; set; }

        public User Owner { get; set; }

        public int AccessLevel { get; set; }

        public bool IsPublished { get; set; }

        public List<CourseState> CourseStates { get; set; }

        public List<Material> Materials { get; set; }

        public List<CourseSkill> Skills { get; set; }
    }
}
