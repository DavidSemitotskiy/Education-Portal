using Portal.Domain.Interfaces;
using Portal.Domain.Models;

namespace Portal.Application.Interfaces
{
    public interface IMaterialStateManager
    {
        IEntityRepository<MaterialState> MaterialStateRepository { get; }

        Task<MaterialState> CreateOrGetExistedMaterialState(MaterialState materialState);

        Task<List<MaterialState>> GetMaterialStatesFromCourse(User user, Course course);

        void CompleteMaterial(MaterialState materialState);
    }
}
