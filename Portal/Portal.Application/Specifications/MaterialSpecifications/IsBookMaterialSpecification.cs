using Portal.Domain.Models;
using Portal.Domain.Specifications;
using System.Linq.Expressions;

namespace Portal.Application.Specifications.MaterialSpecifications
{
    public class IsBookMaterialSpecification : Specification<Material>
    {
        public override Expression<Func<Material, bool>> ToExpression()
        {
            return material => material is BookMaterial;
        }
    }
}
