using FluentValidation;
using Portal.Domain.Models;

namespace Portal.ConsoleAPI.Validation
{
    public class ArticleMaterialValidator : AbstractValidator<ArticleMaterial>
    {
        public ArticleMaterialValidator()
        {
            RuleFor(material => material.Resource).NotEmpty().Must(resource => resource.StartsWith("https://"));
        }
    }
}
