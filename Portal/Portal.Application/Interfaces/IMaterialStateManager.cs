using Portal.Domain.Interfaces;
using Portal.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Portal.Application.Interfaces
{
    public interface IMaterialStateManager
    {
        IEntityRepository<MaterialState> MaterialStateRepository { get; }

        Task<MaterialState> CreateOrGetExistedMaterialState(MaterialState materialState);

        Task<List<MaterialState>> GetMaterialStatesFromCourse(User user, Course course);
    }
}
