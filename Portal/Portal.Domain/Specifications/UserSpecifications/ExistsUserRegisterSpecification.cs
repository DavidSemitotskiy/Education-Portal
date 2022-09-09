using Portal.Domain.DTOs;
using Portal.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Portal.Domain.Specifications.UserSpecifications
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
