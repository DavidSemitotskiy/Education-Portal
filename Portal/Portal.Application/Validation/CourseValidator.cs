using FluentValidation;
using Portal.Domain.Models;

namespace Portal.Application.Validation
{
    public class CourseValidator : AbstractValidator<Course>
    {
        public CourseValidator()
        {
            RuleFor(course => course.Name).Cascade(CascadeMode.StopOnFirstFailure).NotEmpty().Length(1, 100);
            RuleFor(course => course.Description).Cascade(CascadeMode.StopOnFirstFailure).NotEmpty().MaximumLength(100);
            RuleFor(course => course.AccessLevel).Must(access => access >= 0);
        }
    }
}
