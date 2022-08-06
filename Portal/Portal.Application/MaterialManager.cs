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
