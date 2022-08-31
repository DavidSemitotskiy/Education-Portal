using Portal.Domain.Interfaces;
using Portal.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Portal.Application.Interfaces
{
    public interface ICourseStateManager
    {
        IEntityRepository<CourseState> CourseStateRepository { get; }

        IMaterialStateManager MaterialStateManager { get; }

        IUserManager UserManager { get; }

        IUserSkillManager UserSkillManager { get; }

        Task<bool> Exists(User user, CourseState courseState);

        Task Subscribe(User user, Course course);

        void UnSubscribe(CourseState courseState);

        void CompleteMaterialState(MaterialState materialState);

        Task<bool> CheckIfCourseCompleted(User user, Course course, CourseState courseState);

        Task<string> GetCourseProgress(CourseState courseState);
    }
}
