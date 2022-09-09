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

        private readonly string _hashedPassword;

        public ExistsUserLogInSpecification(UserLoginDTO userLogInDTO, string hashedPassword)
        {
            _userLogInDTO = userLogInDTO;
            _hashedPassword = hashedPassword;
        }

        public override Expression<Func<User, bool>> ToExpression()
        {
            return user => user.Email == _userLogInDTO.Email && user.Password == _hashedPassword;
        }
    }
}
