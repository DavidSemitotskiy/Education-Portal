using Portal.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Portal.Domain.Specifications.CourseSpecifications
{
    public class AvailableUserCourseSpecification : Specification<Course>
    {
        private readonly User _user;

        public AvailableUserCourseSpecification(User user)
        {
            _user = user ?? throw new ArgumentNullException("User can't be null");
        }

        public override Expression<Func<Course, bool>> ToExpression()
        {
            return course => course.AccessLevel <= _user.AccessLevel && course.IsPublished;
        }
    }
}
