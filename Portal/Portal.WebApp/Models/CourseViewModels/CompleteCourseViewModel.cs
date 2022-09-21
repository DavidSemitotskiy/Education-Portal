using Portal.Domain.Models;

namespace Portal.WebApp.Models.CourseViewModels
{
    public class CompleteCourseViewModel
    {
        public MaterialState MaterialState { get; set; }

        public string MaterialName { get; set; }

        public Guid CourseStateId { get; set; }
    }
}
