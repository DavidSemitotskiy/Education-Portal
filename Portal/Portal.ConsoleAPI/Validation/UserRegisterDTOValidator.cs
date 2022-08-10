using FluentValidation;
using Portal.Domain.DTOs;

namespace Portal.ConsoleAPI.Validation
{
    public class UserRegisterDTOValidator : AbstractValidator<UserRegisterDTO>
    {
        public UserRegisterDTOValidator()
        {
            RuleFor(user => user.FirstName).NotEmpty();
            RuleFor(user => user.LastName).NotEmpty();
            RuleFor(user => user.Password).NotEmpty();
            RuleFor(user => user.ConfirmPassword).NotEmpty();
            RuleFor(user => user.Email).EmailAddress();
        }
    }
}
