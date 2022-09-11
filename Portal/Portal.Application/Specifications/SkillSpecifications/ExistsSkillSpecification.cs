using Portal.Domain.Models;
using Portal.Domain.Specifications;
using System.Linq.Expressions;

namespace Portal.Application.Specifications.SkillSpecifications
{
    public class ExistsSkillSpecification : Specification<Skill>
    {
        private readonly Skill _skill;

        public ExistsSkillSpecification(Skill skill)
        {
            _skill = skill;
        }

        public override Expression<Func<Skill, bool>> ToExpression()
        {
            return skill => skill.Experience == _skill.Experience;
        }
    }
}
