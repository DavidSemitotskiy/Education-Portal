using Portal.Domain.Interfaces;

namespace Portal.Domain.Models
{
    public class Course
    {
        public Guid IdCourse { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public User Owner { get; set; }

        public int AccessLevel { get; set; }

        public List<Material>? Materials { get; set; }

        public List<User> Subscribers { get; set; }

        public List<Skill> Skills { get; set; }

        public override bool Equals(object? obj)
        {
            if (obj is Course other)
            {
                return GetHashCode() == other.GetHashCode();
            }

            return false;
        }

        public override int GetHashCode()
        {
            return IdCourse.GetHashCode() + Name.GetHashCode() + Description.GetHashCode();
        }
    }
}
