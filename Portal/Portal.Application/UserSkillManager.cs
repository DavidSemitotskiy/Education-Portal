using Portal.Application.Interfaces;
using Portal.Domain.Interfaces;
using Portal.Domain.Models;

namespace Portal.Application
{
    public class UserSkillManager : IUserSkillManager
    {
        public UserSkillManager(IEntityRepository<UserSkill> repository)
        {
            UserSkillRepository = repository ?? throw new ArgumentNullException("Repository can't be null");
        }

        public IEntityRepository<UserSkill> UserSkillRepository { get; }

        public void AddUserSkill(User user, CourseSkill courseSkill)
        {
            if (user == null || courseSkill == null)
            {
                throw new ArgumentNullException("User or CourseSkill can't be null");
            }

            var certainSkill = user.Skills.FirstOrDefault(s => s.Experience == courseSkill.Experience);
            if (certainSkill == null)
            {
                var skill = new UserSkill()
                {
                    Id = Guid.NewGuid(),
                    Experience = courseSkill.Experience,
                    Owner = user,
                    Level = 0
                };
                user.Skills.Add(skill);
                return;
            }

            certainSkill.Level++;
        }
    }
}
