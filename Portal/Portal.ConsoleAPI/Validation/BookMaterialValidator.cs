using FluentValidation;
using Portal.Domain.Models;

namespace Portal.ConsoleAPI.Validation
{
    public class BookMaterialValidator : AbstractValidator<BookMaterial>
    {
        public BookMaterialValidator()
        {
            RuleFor(material => material.Authors).NotEmpty();
            RuleFor(material => material.Title).NotEmpty();
            RuleFor(material => material.CountPages).Must(count => count > 0);
            RuleFor(material => material.Format).NotEmpty();
        }
    }
}
