using Portal.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Portal.Domain.Specifications.CourseSpecifications
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
            return course => course.OwnerUser == _user.UserId;
        }
    }
}
