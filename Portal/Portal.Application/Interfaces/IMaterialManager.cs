using Portal.Domain.Interfaces;
using Portal.Domain.Models;

namespace Portal.Application.Interfaces
{
    public interface IMaterialManager
    {
        IEntityRepository<Material> MaterialRepository { get; }

        Task AddMaterial(Material material);

        Task DeleteMaterial(Material material);

        Task UpdateMaterial(Material material);

        Task<bool> Exists(Material material);
    }
}
