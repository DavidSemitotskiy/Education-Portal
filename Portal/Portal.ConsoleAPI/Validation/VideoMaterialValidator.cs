using FluentValidation;
using Portal.Domain.Models;

namespace Portal.ConsoleAPI.Validation
{
    public class VideoMaterialValidator : AbstractValidator<VideoMaterial>
    {
        public VideoMaterialValidator()
        {
            RuleFor(material => material.Duration).Must(duration => duration > 0);
            RuleFor(material => material.Quality).Cascade(CascadeMode.StopOnFirstFailure).NotEmpty().Matches("^(?:144p|240p|360p|720p60|1080p60|1440p60)$");
        }
    }
}
