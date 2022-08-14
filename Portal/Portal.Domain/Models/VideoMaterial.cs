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

        public override bool Equals(object? obj)
        {
            if (obj is VideoMaterial other)
            {
                return GetHashCode() == other.GetHashCode();
            }

            return false;
        }

        public override int GetHashCode()
        {
            return Duration.GetHashCode() + Quality.GetHashCode();
        }
    }
}
