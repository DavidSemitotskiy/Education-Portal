using Portal.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Portal.Domain.Specifications.MaterialSpecifications
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
            return state => state.OwnerMaterial == _materialState.OwnerMaterial && state.UserId == _materialState.UserId;
        }
    }
}
