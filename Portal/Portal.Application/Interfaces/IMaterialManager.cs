using Portal.Domain.Interfaces;

namespace Portal.Application.Interfaces
{
    public interface IMaterialManager
    {
        IMaterialRepository MaterialRepository { get; set; }

        void AddMaterial(Material material);

        void DeleteMaterial(Material material);
    }
}
