using Portal.Domain.Interfaces;

namespace Portal.Domain.Models
{
    public class VideoMaterial : Material
    {
        public long Duration { get; set; }

        public string? Quality { get; set; }

        public override string ToString()
        {
            return $"Duration - {Duration} : Quality - {Quality}";
        }
    }
}
