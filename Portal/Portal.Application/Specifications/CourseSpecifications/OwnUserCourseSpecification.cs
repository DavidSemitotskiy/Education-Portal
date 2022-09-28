using Portal.Domain.Models;
using Portal.Domain.Specifications;
using System.Linq.Expressions;

namespace Portal.Application.Specifications.CourseSpecifications
{
    public class OwnUserCourseSpecification : Specification<Course>
    {
        private readonly User _user;

        public OwnUserCourseSpecification(User user)
        {
            _user = user ?? throw new ArgumentNullException("User can't be null");
        }

        public override Expression<Func<Course, bool>> ToExpression()
        {
            return course => course.OwnerUser == _user.UserName;
        }
    }
}
