using Portal.Domain.Models;
using Portal.Domain.Specifications;
using System.Linq.Expressions;

namespace Portal.Application.Specifications.MaterialSpecifications
{
    public class IsArticleMaterialSpecification : Specification<Material>
    {
        public override Expression<Func<Material, bool>> ToExpression()
        {
            return material => material is ArticleMaterial;
        }
    }
}
