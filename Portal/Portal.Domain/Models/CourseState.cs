using Portal.Domain.BaseModels;

namespace Portal.Domain.Models
{
    public class CourseState : Entity
    {
        public Course OwnerCourse { get; set; }

        public Guid UserId { get; set; }

        public User OwnerUser { get; set; }

        public bool IsFinished { get; set; }

        public List<MaterialState> MaterialStates { get; set; }
    }
}
