using Portal.Application.Interfaces;
using Portal.Domain.Interfaces;
using Portal.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Portal.Application
{
    public class MaterialManager : IMaterialManager
    {
        public MaterialManager(IEntityRepository<Material> repository)
        {
            MaterialRepository = repository ?? throw new ArgumentNullException("Repository can't be null");
        }

        public IEntityRepository<Material> MaterialRepository { get; }

        public async Task AddMaterial(Material material)
        {
            if (material == null)
            {
                throw new ArgumentNullException("Course can't be null");
            }

            if (await Exists(material))
            {
                throw new ArgumentException("This material already exists");
            }

            await MaterialRepository.Add(material);
        }

        public void DeleteMaterial(Material material)
        {
            MaterialRepository.Delete(material);
        }

        public async Task<bool> Exists(Material material)
        {
            var allMaterials = await MaterialRepository.GetAllEntities();
            return allMaterials.Any(m => m.Equals(material));
        }

        public void UpdateMaterial(Material material)
        {
            MaterialRepository.Update(material);
        }
    }
}
