using Portal.Application.Interfaces;
using Portal.Domain.Interfaces;

namespace Portal.Application
{
    public class MaterialManager : IMaterialManager
    {
        public MaterialManager(IMaterialRepository materialRepository)
        {
            MaterialRepository = materialRepository ?? throw new ArgumentNullException("Repository can't be null");
        }

        public IMaterialRepository MaterialRepository { get; set; }

        public IEnumerable<Material> GetAllMaterials()
        {
            return MaterialRepository.Materials;
        }

        public void AddMaterial(Material material)
        {
            if (material == null)
            {
                throw new ArgumentNullException("Course can't be null");
            }

            if (MaterialRepository.Exists(material))
            {
                throw new ArgumentException("This material already exists");
            }

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
