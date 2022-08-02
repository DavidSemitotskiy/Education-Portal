using Portal.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Portal.Domain.Models
{
    public class Course
    {
        public int IdCourse { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public int AccessLevel { get; set; }

        public List<Material>? IdCourses { get; set; }
    }
}
