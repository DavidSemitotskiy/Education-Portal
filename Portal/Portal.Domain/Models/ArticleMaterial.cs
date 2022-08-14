namespace Portal.Domain.Models
{
    public class ArticleMaterial : Material
    {
        public DateTime DatePublication { get; set; }

        public string? Resource { get; set; }

        public override string ToString()
        {
            return $"Resource: {Resource} - ({DatePublication.ToString("d")})";
        }

        public override bool Equals(object? obj)
        {
            if (obj is ArticleMaterial other)
            {
                return GetHashCode() == other.GetHashCode();
            }

            return false;
        }

        public override int GetHashCode()
        {
            return DatePublication.GetHashCode() + Resource.GetHashCode();
        }
    }
}
