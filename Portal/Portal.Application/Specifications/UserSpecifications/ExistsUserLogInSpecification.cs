using Portal.Domain.DTOs;
using Portal.Domain.Models;
using Portal.Domain.Specifications;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Portal.Application.Specifications.UserSpecifications
{
    public class ExistsUserLogInSpecification : Specification<User>
    {
        private readonly UserLoginDTO _userLogInDTO;

        public ExistsUserLogInSpecification(UserLoginDTO userLogInDTO)
        {
            _userLogInDTO = userLogInDTO;
        }

        public override Expression<Func<User, bool>> ToExpression()
        {
            var hashedPassword = AccountService.GetHashPassword(_userLogInDTO.Password);
            return user => user.Email == _userLogInDTO.Email && user.Password == hashedPassword;
        }
    }
}
