using FluentValidation;
using Portal.Domain.DTOs;

namespace Portal.ConsoleAPI.Validation
{
    public class UserLoginDTOValidator : AbstractValidator<UserLoginDTO>
    {
        public UserLoginDTOValidator()
        {
            RuleFor(user => user.Email).EmailAddress();
            RuleFor(user => user.Password).NotEmpty();
        }
    }
}
