using Portal.Domain.Models;
using Portal.Domain.Specifications;
using System.Linq.Expressions;

namespace Portal.Application.Specifications.CourseSpecifications
{
    public class SearchCourseBySearchStringSpecification : Specification<Course>
    {
        private readonly string _searchString;

        public SearchCourseBySearchStringSpecification(string searchString)
        {
            _searchString = searchString;
        }

        public override Expression<Func<Course, bool>> ToExpression()
        {
            return course => course.Name.Contains(_searchString);
        }
    }
}
