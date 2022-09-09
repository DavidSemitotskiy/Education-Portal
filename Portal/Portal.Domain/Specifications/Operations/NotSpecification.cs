using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

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
            var candidateExpr = expression.Parameters[0];
            var body = Expression.Not(expression.Body);
            var finalExpr = Expression.Lambda<Func<T, bool>>(body, candidateExpr);
            return finalExpr;
        }
    }
}
