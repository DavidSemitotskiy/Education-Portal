namespace Portal.Domain.Models
{
    public class User
    {
        public Guid UserId { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Password { get; set; }

        public string Email { get; set; }

        public int AccessLevel { get; set; }

        public List<CourseState> CourseStates { get; set; }

        public List<UserSkill> Skills { get; set; }

        public List<Course> OwnCourses { get; set; }
    }
}
