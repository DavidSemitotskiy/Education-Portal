using Portal.Domain.Models;
using Portal.Domain.Specifications;
using System.Linq.Expressions;

namespace Portal.Application.Specifications.CourseSpecifications
{
    public class ExistsCourseStateSpecification : Specification<CourseState>
    {
        private readonly CourseState _courseState;

        public ExistsCourseStateSpecification(CourseState courseState)
        {
            _courseState = courseState;
        }

        public override Expression<Func<CourseState, bool>> ToExpression()
        {
            return state => state.CourseId == _courseState.CourseId && state.OwnerUser == _courseState.OwnerUser;
        }
    }
}
