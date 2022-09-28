using Portal.Domain.Interfaces;
using Portal.Domain.Models;

namespace Portal.Application.Interfaces
{
    public interface IMaterialManager
    {
        IEntityRepository<Material> MaterialRepository { get; }

        Task<Material> CreateOrGetExistedMaterial(Material material);

        void DeleteMaterial(Course course, Material material);

        void UpdateMaterial(Course course, int index, Material material);

        Task<bool> Exists(Material material);

        Task<List<Material>> GetMaterialsByPageWithFilterString(FilterTypeMaterial filterMaterial, int page, int pageSize);

        Task<int> TotalCountOfMaterialsWithFilterString(FilterTypeMaterial filterMaterial);
    }
}
