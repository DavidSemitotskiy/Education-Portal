using Portal.Domain.Specifications.Operations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Portal.Domain.Specifications
{
    public abstract class Specification<T>
    {
        public abstract Expression<Func<T, bool>> ToExpression();

        public bool IsSatisfiedBy(T item)
        {
            var predicate = ToExpression().Compile();
            return predicate(item);
        }

        public Specification<T> And(Specification<T> otherSpecification)
        {
            return new AndSpecification<T>(this, otherSpecification);
        }

        public Specification<T> Not()
        {
            return new NotSpecification<T>(this);
        }
    }
}
