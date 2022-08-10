using FluentValidation;
using Portal.Domain.DTOs;

namespace Portal.ConsoleAPI.Validation
{
    public class UserRegisterDTOValidator : AbstractValidator<UserRegisterDTO>
    {
        public UserRegisterDTOValidator()
        {
            RuleFor(user => user.FirstName).NotNull().NotEmpty();
            RuleFor(user => user.LastName).NotNull().NotEmpty();
            RuleFor(user => user.Password).NotNull().NotEmpty();
            RuleFor(user => user.ConfirmPassword).NotNull().NotEmpty();
            RuleFor(user => user.Email).EmailAddress();
        }
    }
}
