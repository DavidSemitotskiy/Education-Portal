using System.Linq.Expressions;

namespace Portal.Domain.Specifications.Operations
{
    public class NotSpecification<T> : Specification<T>
    {
        private readonly Specification<T> _specification;

        public NotSpecification(Specification<T> specification)
        {
            _specification = specification;
        }

        public override Expression<Func<T, bool>> ToExpression()
        {
            Expression<Func<T, bool>> expression = _specification.ToExpression();
            var candidateExpression = expression.Parameters[0];
            var body = Expression.Not(expression.Body);
            var finalExpr = Expression.Lambda<Func<T, bool>>(body, candidateExpression);
            return finalExpr;
        }
    }
}
