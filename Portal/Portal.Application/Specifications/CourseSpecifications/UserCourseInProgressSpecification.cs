using Portal.Domain.Models;
using Portal.Domain.Specifications;
using System.Linq.Expressions;

namespace Portal.Application.Specifications.CourseSpecifications
{
    public class UserCourseInProgressSpecification : Specification<CourseState>
    {
        private readonly User _user;

        public UserCourseInProgressSpecification(User user)
        {
            _user = user ?? throw new ArgumentNullException("User can't be null");
        }

        public override Expression<Func<CourseState, bool>> ToExpression()
        {
            return courseState => courseState.OwnerUser == _user.UserName;
        }
    }
}
