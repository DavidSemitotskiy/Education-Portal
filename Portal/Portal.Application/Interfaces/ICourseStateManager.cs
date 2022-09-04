using Portal.Domain.Interfaces;
using Portal.Domain.Models;

namespace Portal.Application.Interfaces
{
    public interface ICourseStateManager
    {
        IEntityRepository<CourseState> CourseStateRepository { get; }

        IMaterialStateManager MaterialStateManager { get; }

        IUserManager UserManager { get; }

        IUserSkillManager UserSkillManager { get; }

        Task<bool> Exists(User user, CourseState courseState);

        Task<CourseState> Subscribe(User user, Course course);

        void UnSubscribe(CourseState courseState);

        void CompleteMaterialState(MaterialState materialState);

        Task<bool> CheckIfCourseCompleted(User user, Course course, CourseState courseState);

        Task<string> GetCourseProgress(CourseState courseState);
    }
}
