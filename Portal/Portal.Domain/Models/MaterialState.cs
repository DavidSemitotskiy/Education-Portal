using Portal.Domain.BaseModels;

namespace Portal.Domain.Models
{
    public class MaterialState : Entity
    {
        public List<CourseState> CourseStates { get; set; }

        public Guid OwnerMaterial { get; set; }

        public string OwnerUser { get; set; }

        public bool IsCompleted { get; set; }
    }
}
