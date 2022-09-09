using Portal.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Portal.Domain.Specifications.MaterialStateSpecifications
{
    public class CompleteMaterialStateSpecification : Specification<MaterialState>
    {
        public override Expression<Func<MaterialState, bool>> ToExpression()
        {
            return materialState => materialState.IsCompleted;
        }
    }
}
