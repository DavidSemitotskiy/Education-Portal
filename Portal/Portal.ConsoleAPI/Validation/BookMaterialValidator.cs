using FluentValidation;
using Portal.Domain.Models;

namespace Portal.ConsoleAPI.Validation
{
    public class BookMaterialValidator : AbstractValidator<BookMaterial>
    {
        public BookMaterialValidator()
        {
            RuleFor(material => material.Authors).Cascade(CascadeMode.StopOnFirstFailure).NotEmpty().Length(5, 100);
            RuleFor(material => material.Title).Cascade(CascadeMode.StopOnFirstFailure).NotEmpty().Length(1, 100);
            RuleFor(material => material.CountPages).Must(count => count > 0);
            RuleFor(material => material.Format).Cascade(CascadeMode.StopOnFirstFailure).NotEmpty().Matches("^(?:pdf|doc)$");
        }
    }
}
