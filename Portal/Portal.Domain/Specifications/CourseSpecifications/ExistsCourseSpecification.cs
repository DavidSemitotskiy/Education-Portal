using Portal.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Portal.Domain.Specifications.CourseSpecifications
{
    public class ExistsCourseSpecification : Specification<Course>
    {
        private readonly Course _course;

        public ExistsCourseSpecification(Course course)
        {
            _course = course;
        }

        public override Expression<Func<Course, bool>> ToExpression()
        {
            return course => course.Name == _course.Name && course.Description == _course.Description;
        }
    }
}
