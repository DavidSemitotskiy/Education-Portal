using Portal.Domain.BaseModels;

namespace Portal.Domain.Models
{
    public class MaterialState : Entity
    {
        public List<CourseState> CourseStates { get; set; }

        public Material OwnerMaterial { get; set; }

        public Guid UserId { get; set; }

        public User OwnerUser { get; set; }

        public bool IsCompleted { get; set; }
    }
}
