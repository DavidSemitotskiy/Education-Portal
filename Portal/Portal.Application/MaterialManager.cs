using Portal.Application.Interfaces;
using Portal.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Portal.Application
{
    internal class MaterialManager : IMaterialManager
    {
        public MaterialManager(IMaterialRepository materialRepository)
        {
            MaterialRepository = materialRepository ?? throw new ArgumentNullException("Repository can't be null");
        }

        public IMaterialRepository MaterialRepository { get; set; }

        public IEnumerable<Material> GetAllMaterials()
        {
            return MaterialRepository.GetAllMaterials();
        }

        public void AddMaterial(Material material)
        {
            MaterialRepository.Add(material);
            //MaterialRepository.SaveChanges();
        }

        public void DeleteMaterial(Material material)
        {
            MaterialRepository.Delete(material);
            MaterialRepository.SaveChanges();
        }
    }
}
