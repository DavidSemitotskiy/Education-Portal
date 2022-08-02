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
        public List<string> Authors { get; set; }

        public int CountPages { get; set; }

        public string Format { get; set; }

        public string YearPublication { get; set; }
    }
}
