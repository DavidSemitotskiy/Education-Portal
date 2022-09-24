using Portal.Application.Interfaces;
using Portal.Application.Specifications.MaterialSpecifications;
using Portal.Domain.Interfaces;
using Portal.Domain.Models;
using Portal.Domain.Specifications;

namespace Portal.Application
{
    public class MaterialManager : IMaterialManager
    {
        public MaterialManager(IEntityRepository<Material> repository)
        {
            MaterialRepository = repository ?? throw new ArgumentNullException("Repository can't be null");
        }

        public IEntityRepository<Material> MaterialRepository { get; }

        public async Task<Material> CreateOrGetExistedMaterial(Material material)
        {
            var allMaterials = await MaterialRepository.GetAllEntities();
            var certainMaterial = allMaterials.FirstOrDefault(m => m.Equals(material));
            if (certainMaterial == null)
            {
                await MaterialRepository.Add(material);
                return material;
            }

            return certainMaterial;
        }

        public void DeleteMaterial(Course course, Material material)
        {
            if (course == null)
            {
                throw new ArgumentNullException("Course can't be null");
            }

            course.Materials.Remove(material);
        }

        public async Task<bool> Exists(Material material)
        {
            var allMaterials = await MaterialRepository.GetAllEntities();
            return allMaterials.Any(m => m.Equals(material));
        }

        public void UpdateMaterial(Course course, int index, Material material)
        {
            if (course == null)
            {
                throw new ArgumentNullException("Course can't be null");
            }

            course.Materials[index] = material;
        }

        public Task<int> TotalCountOfMaterialsWithFilterString(string filterString)
        {
            var specification = GetSpecificationWithFilterString(filterString);
            return specification == null ? MaterialRepository.TotalCountOfEntities()
                : MaterialRepository.TotalCountOfEntitiesBySpecification(specification);
        }

        public Task<List<Material>> GetMaterialsByPageWithFilterString(string filterString, int page, int pageSize)
        {
            var specification = GetSpecificationWithFilterString(filterString);
            return specification == null ? MaterialRepository.GetEntitiesFromPage(page, pageSize)
                : MaterialRepository.GetEntitiesBySpecificationFromPage(page, pageSize, specification);
        }

        private Specification<Material> GetSpecificationWithFilterString(string filterString)
        {
            Specification<Material> specification = null;
            switch (filterString)
            {
                case "All":
                    return specification;
                case "Books":
                    specification = new IsBookMaterialSpecification();
                    break;
                case "Videos":
                    specification = new IsVideoMaterialSpecification();
                    break;
                case "Articles":
                    specification = new IsArticleMaterialSpecification();
                    break;
            }

            return specification;
        }
    }
}
