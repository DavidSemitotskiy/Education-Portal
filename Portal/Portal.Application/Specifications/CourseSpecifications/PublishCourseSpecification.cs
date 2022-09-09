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
    public class PublishCourseSpecification : Specification<Course>
    {
        public override Expression<Func<Course, bool>> ToExpression()
        {
            return course => course.IsPublished;
        }
    }
}
