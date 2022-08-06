using Portal.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Portal.Domain.Interfaces
{
    public interface ICourseRepository
    {
        IEnumerable<Course> GetAllCourses();

        void Add(Course course);

        void Delete(Course course);

        void Update(Course course);

        bool Exists(string name, string description);

        void SaveChanges();
    }
}
