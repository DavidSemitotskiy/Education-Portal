using Portal.Domain.Models;
using Portal.Domain.Specifications;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

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
