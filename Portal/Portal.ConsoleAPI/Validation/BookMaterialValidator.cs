using FluentValidation;
using Portal.Domain.Models;

namespace Portal.ConsoleAPI.Validation
{
    public class BookMaterialValidator : AbstractValidator<BookMaterial>
    {
        public BookMaterialValidator()
        {
            RuleFor(material => material.Authors).NotNull().NotEmpty();
            RuleFor(material => material.Title).NotNull().NotEmpty();
            RuleFor(material => material.CountPages).Must(count => count > 0);
            RuleFor(material => material.Format).NotNull().NotEmpty();
        }
    }
}
