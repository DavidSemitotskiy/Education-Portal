using FluentValidation;
using Portal.Domain.DTOs;

namespace Portal.Application.Validation
{
    public class UserLoginDTOValidator : AbstractValidator<UserLoginDTO>
    {
        public UserLoginDTOValidator()
        {
            RuleFor(user => user.Email).NotNull().EmailAddress();
            RuleFor(user => user.Password).NotEmpty();
        }
    }
}
