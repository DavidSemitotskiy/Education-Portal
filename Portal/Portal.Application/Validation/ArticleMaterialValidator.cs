using FluentValidation;
using Portal.Domain.Models;

namespace Portal.Application.Validation
{
    public class ArticleMaterialValidator : AbstractValidator<ArticleMaterial>
    {
        public ArticleMaterialValidator()
        {
            RuleFor(material => material.Resource).Cascade(CascadeMode.StopOnFirstFailure).NotEmpty().
                Must(resource => resource.StartsWith("https://")).WithMessage("Resource must start with https://").MinimumLength(15);
        }
    }
}
