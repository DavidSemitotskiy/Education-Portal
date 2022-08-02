using Portal.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Portal.Domain.Models
{
    public class VideoMaterial : Material
    {
        public long Duration { get; set; }

        public string? Quality { get; set; }
    }
}
