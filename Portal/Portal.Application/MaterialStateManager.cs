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
    public class MaterialStateManager : IMaterialStateManager
    {
        public MaterialStateManager(IEntityRepository<MaterialState> materialStateRepository)
        {
            MaterialStateRepository = materialStateRepository;
        }

        public IEntityRepository<MaterialState> MaterialStateRepository { get; }

        public async Task<MaterialState> CreateOrGetExistedMaterialState(MaterialState materialState)
        {
            var allMaterialStates = await MaterialStateRepository.GetAllEntities();
            var certainMaterialState = allMaterialStates.FirstOrDefault(state => state.OwnerMaterial == materialState.OwnerMaterial && state.UserId == materialState.UserId);
            if (certainMaterialState == null)
            {
                await MaterialStateRepository.Add(materialState);
                return materialState;
            }

            return certainMaterialState;
        }

        public async Task<List<MaterialState>> GetMaterialStatesFromCourse(User user, Course course)
        {
            if (course == null || user == null)
            {
                throw new ArgumentNullException("User and course can't be null");
            }

            MaterialState materialState = null;
            var materialStates = new List<MaterialState>();
            for (int i = 0; i < course.Materials.Count; i++)
            {
                materialState = new MaterialState
                {
                    Id = Guid.NewGuid(),
                    UserId = user.UserId,
                    IsCompleted = false,
                    OwnerMaterial = course.Materials[i].Id,
                    CourseStates = new List<CourseState>()
                };
                materialState = await CreateOrGetExistedMaterialState(materialState);
                materialStates.Add(materialState);
            }

            return materialStates;
        }

        public void CompleteMaterial(MaterialState materialState)
        {
            if (materialState == null)
            {
                throw new ArgumentNullException("MaterialState can't be null");
            }

            materialState.IsCompleted = true;
            MaterialStateRepository.Update(materialState);
        }
    }
}
