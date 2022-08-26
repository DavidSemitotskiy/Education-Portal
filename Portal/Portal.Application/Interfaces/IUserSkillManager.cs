using Portal.Domain.Interfaces;
using Portal.Domain.Models;

namespace Portal.Application.Interfaces
{
    public interface IUserSkillManager
    {
        IEntityRepository<UserSkill> UserSkillRepository { get; }

        void AddUserSkill(User user, CourseSkill courseSkill);
    }
}
