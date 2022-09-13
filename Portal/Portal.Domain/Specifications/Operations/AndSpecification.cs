using System.Linq.Expressions;

namespace Portal.Domain.Specifications.Operations
{
    public class AndSpecification<T> : Specification<T>
    {
        private readonly Specification<T> _leftSpecification;

        private readonly Specification<T> _rightSpecification;

        public AndSpecification(Specification<T> leftSpecification, Specification<T> rightSpecification)
        {
            _leftSpecification = leftSpecification;
            _rightSpecification = rightSpecification;
        }

        public override Expression<Func<T, bool>> ToExpression()
        {
            Expression<Func<T, bool>> leftExpression = _leftSpecification.ToExpression();
            Expression<Func<T, bool>> rightExpression = _rightSpecification.ToExpression();

            var paramExpression = Expression.Parameter(typeof(T));
            var expressionBody = Expression.AndAlso(leftExpression.Body, rightExpression.Body);
            expressionBody = (BinaryExpression)new ParameterReplacer(paramExpression).Visit(expressionBody);
            var finalExpr = Expression.Lambda<Func<T, bool>>(expressionBody, paramExpression);

            return finalExpr;
        }
    }
}
