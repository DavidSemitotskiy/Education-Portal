using Portal.Domain.Models;
using Portal.Domain.Specifications;
using System.Linq.Expressions;

namespace Portal.Application.Specifications.MaterialSpecifications
{
    public class CompleteMaterialStateSpecification : Specification<MaterialState>
    {
        public override Expression<Func<MaterialState, bool>> ToExpression()
        {
            return materialState => materialState.IsCompleted;
        }
    }
}
