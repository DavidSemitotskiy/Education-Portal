using Portal.Domain.Models;
using Portal.Domain.Specifications;
using System.Linq.Expressions;

namespace Portal.Application.Specifications.MaterialSpecifications
{
    public class ExistsMaterialStateSpecification : Specification<MaterialState>
    {
        private readonly MaterialState _materialState;

        public ExistsMaterialStateSpecification(MaterialState materialState)
        {
            _materialState = materialState;
        }

        public override Expression<Func<MaterialState, bool>> ToExpression()
        {
            return state => state.OwnerMaterial == _materialState.OwnerMaterial && state.OwnerUser == _materialState.OwnerUser;
        }
    }
}
