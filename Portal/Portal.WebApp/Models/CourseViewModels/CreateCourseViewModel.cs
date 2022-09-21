using System.ComponentModel.DataAnnotations;

namespace Portal.WebApp.Models.CourseViewModels
{
    public class CreateCourseViewModel
    {
        [Required]
        public string Name { get; set; }

        [Required]
        public string Description { get; set; }

        public int AccessLevel { get; set; }
    }
}
