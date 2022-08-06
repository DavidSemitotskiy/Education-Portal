using Portal.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
