using Portal.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Portal.Domain.Specifications.CourseSpecifications
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
            return state => state.CourseId == _courseState.CourseId && state.UserId == _courseState.UserId;
        }
    }
}
