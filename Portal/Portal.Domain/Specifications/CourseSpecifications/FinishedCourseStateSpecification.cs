using Portal.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Portal.Domain.Specifications.CourseSpecifications
{
    public class FinishedCourseStateSpecification : Specification<CourseState>
    {
        public override Expression<Func<CourseState, bool>> ToExpression()
        {
            return courseState => courseState.IsFinished;
        }
    }
}
