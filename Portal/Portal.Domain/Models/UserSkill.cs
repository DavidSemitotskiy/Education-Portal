namespace Portal.Domain.Models
{
    public class UserSkill : Skill
    {
        public int Level { get; set; }

        public User Owner { get; set; }
    }
}
