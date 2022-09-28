using Portal.Domain.BaseModels;

namespace Portal.Domain.Models
{
    public class CourseState : Entity
    {
        public Guid CourseId { get; set; }

        public string OwnerUser { get; set; }

        public bool IsFinished { get; set; }

        public List<MaterialState> MaterialStates { get; set; }
    }
}
