using FluentValidation;
using Portal.Domain.DTOs;
using System.Text.RegularExpressions;

namespace Portal.Application.Validation
{
    public class UserRegisterDTOValidator : AbstractValidator<UserRegisterDTO>
    {
        public UserRegisterDTOValidator()
        {
            RuleFor(user => user.FirstName).Cascade(CascadeMode.StopOnFirstFailure).NotEmpty().Length(1, 20);
            RuleFor(user => user.LastName).Cascade(CascadeMode.StopOnFirstFailure).NotEmpty().Length(1, 20);
            RuleFor(user => user.Password).Cascade(CascadeMode.StopOnFirstFailure).NotEmpty().MinimumLength(6)
                .Matches(@"^(?=.*\d)(?=.*[a-z])(?=.*[A-Z])[a-zA-Z0-9]{6,}");
            RuleFor(user => user).Must(user => user.Password == user.ConfirmPassword)
                .When(user => !string.IsNullOrEmpty(user.Password) && Regex.IsMatch(user.Password, @"^(?=.*\d)(?=.*[a-z])(?=.*[A-Z])[a-zA-Z0-9]{6,}"))
                .WithMessage("Confirm password must equal Password.");
            RuleFor(user => user.Email).Cascade(CascadeMode.StopOnFirstFailure).NotNull().EmailAddress();
        }
    }
}
