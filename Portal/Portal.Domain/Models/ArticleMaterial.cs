using Portal.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
