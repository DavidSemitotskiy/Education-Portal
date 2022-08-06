using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Portal.Domain.Interfaces
{
    public interface IMaterialRepository
    {
        IEnumerable<Material> GetAllMaterials();

        void Add(Material material);

        void Delete(Material material);

        void Update(Material material);

        void SaveChanges();
    }
}
