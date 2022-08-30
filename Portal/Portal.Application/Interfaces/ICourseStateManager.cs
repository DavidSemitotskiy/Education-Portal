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

        Task<bool> Exists(User user, CourseState courseState);

        Task Subscribe(User user, Course course);

        void UnSubscribe(CourseState courseState);

        void CompleteMaterialState(MaterialState materialState);

        Task<string> GetCourseProgress(CourseState courseState);
    }
}
