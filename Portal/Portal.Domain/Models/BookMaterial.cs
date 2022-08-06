using Portal.Domain.Interfaces;

namespace Portal.Domain.Models
{
    public class BookMaterial : Material
    {
        public string Authors { get; set; }

        public string Title { get; set; }

        public int CountPages { get; set; }

        public string Format { get; set; }

        public DateTime DatePublication { get; set; }

        public override string ToString()
        {
            return $"{Authors} - {Title}, {CountPages} ({DatePublication.ToString("d")}).{Format}";
        }
    }
}
