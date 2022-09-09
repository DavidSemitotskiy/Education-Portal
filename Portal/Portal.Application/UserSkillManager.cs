using Portal.Application.Interfaces;
using Portal.Domain.Interfaces;
using Portal.Domain.Models;
using Portal.Domain.Specifications.SkillSpecifications;

namespace Portal.Application
{
    public class UserSkillManager : IUserSkillManager
    {
        public UserSkillManager(IEntityRepository<UserSkill> repository)
        {
            UserSkillRepository = repository ?? throw new ArgumentNullException("Repository can't be null");
        }

        public IEntityRepository<UserSkill> UserSkillRepository { get; }

        public Task AddUserSkill(User user, CourseSkill courseSkill)
        {
            if (user == null || courseSkill == null)
            {
                throw new ArgumentNullException("User or CourseSkill can't be null");
            }

            var existsSkillSpecification = new ExistsSkillSpecification(courseSkill);
            var certainSkill = (UserSkill)user.Skills.FirstOrDefault(existsSkillSpecification.ToExpression().Compile());
            if (certainSkill == null)
            {
                var skill = new UserSkill()
                {
                    Id = Guid.NewGuid(),
                    Experience = courseSkill.Experience,
                    Owner = user,
                    Level = 0
                };
                return UserSkillRepository.Add(skill);
            }

            certainSkill.Level++;
            UserSkillRepository.Update(certainSkill);
            return Task.CompletedTask;
        }
    }
}
