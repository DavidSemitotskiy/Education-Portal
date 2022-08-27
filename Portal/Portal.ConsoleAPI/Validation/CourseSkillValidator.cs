using FluentValidation;
using Portal.Domain.Models;

namespace Portal.ConsoleAPI.Validation
{
    public class CourseSkillValidator : AbstractValidator<CourseSkill>
    {
        public CourseSkillValidator()
        {
            RuleFor(course => course.Experience).NotEmpty();
            RuleFor(course => course.Courses).NotNull();
        }
    }
}
