using Portal.Domain.Interfaces;

namespace Portal.Domain.Models
{
    public class ArticleMaterial : Material
    {
        public DateTime DatePublication { get; set; }

        public string? Resource { get; set; }

        public override string ToString()
        {
            return $"Resource: {Resource} - ({DatePublication.ToString("d")}";
        }
    }
}
