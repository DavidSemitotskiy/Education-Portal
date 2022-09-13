using Portal.Domain.Models;
using Portal.Domain.Specifications;
using System.Linq.Expressions;

namespace Portal.Application.Specifications.CourseSpecifications
{
    public class FinishedCourseStateSpecification : Specification<CourseState>
    {
        public override Expression<Func<CourseState, bool>> ToExpression()
        {
            return courseState => courseState.IsFinished;
        }
    }
}
