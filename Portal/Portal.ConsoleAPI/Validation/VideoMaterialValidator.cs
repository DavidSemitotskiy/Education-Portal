using FluentValidation;
using Portal.Domain.Models;

namespace Portal.ConsoleAPI.Validation
{
    public class VideoMaterialValidator : AbstractValidator<VideoMaterial>
    {
        public VideoMaterialValidator()
        {
            RuleFor(material => material.Duration).Must(duration => duration > 0);
            RuleFor(material => material.Quality).NotNull().NotEmpty();
        }
    }
}
