using Portal.Domain.DTOs;
using Portal.Domain.Models;
using Portal.Domain.Specifications;

namespace Portal.Application.Specifications.UserSpecifications
{
    public class ExistsUserRegisterSpecification : Specification<User>
    {
        private readonly UserRegisterDTO _userRegisterDTO;

        public ExistsUserRegisterSpecification(UserRegisterDTO userRegisterDTO)
        {
            _userRegisterDTO = userRegisterDTO;
        }

        public override System.Linq.Expressions.Expression<Func<User, bool>> ToExpression()
        {
            return user => user.Email == _userRegisterDTO.Email;
        }
    }
}
