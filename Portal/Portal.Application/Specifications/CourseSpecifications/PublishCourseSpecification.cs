using Portal.Domain.Models;
using Portal.Domain.Specifications;
using System.Linq.Expressions;

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
