using Portal.Domain.Models;
using Portal.Domain.Specifications;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

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
            return courseState => courseState.UserId == _user.UserId;
        }
    }
}
