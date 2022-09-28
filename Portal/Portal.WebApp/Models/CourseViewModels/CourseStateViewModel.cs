namespace Portal.WebApp.Models.CourseViewModels
{
    public class CourseStateViewModel
    {
        public Guid Id { get; set; }

        public Guid CourseId { get; set; }

        public string Name { get; set; }

        public string Progress { get; set; }

        public string Description { get; set; }
    }
}
