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
            RuleFor(user => user).Must(user => user.Password == user.ConfirmPassword)
                .When(user => !string.IsNullOrEmpty(user.Password)).WithMessage("Confirm password must equal Password.");
            RuleFor(user => user.Email).EmailAddress();
        }
    }
}
