using Portal.Domain.BaseModels;
using Portal.Domain.Specifications;

namespace Portal.Domain.Interfaces
{
    public interface IEntityRepository<TEntity> where TEntity : Entity
    {
        Task<int> TotalCountOfEntitiesBySpecification(Specification<TEntity> specification);

        Task<int> TotalCountOfEntities();

        Task<List<TEntity>> GetAllEntities();

        Task<List<TEntity>> FindEntitiesBySpecification(Specification<TEntity> specification);

        Task<List<TEntity>> GetEntitiesBySpecificationFromPage(int page, int pageSize, Specification<TEntity> specification);

        Task<List<TEntity>> GetEntitiesFromPage(int page, int pageSize);

        Task<TEntity> FindById(Guid id);

        Task<TEntity> FindByIdWithIncludesAsync(Guid id, string[] includeNames);

        Task Add(TEntity entity);

        void Delete(TEntity entity);

        void Update(TEntity entity);

        Task<int> SaveChanges();
    }
}
