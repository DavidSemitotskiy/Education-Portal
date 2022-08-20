using Portal.Domain.BaseModels;

namespace Portal.Domain.Interfaces
{
    public interface IEntityRepository<TEntity> where TEntity : Entity
    {
        Task<List<TEntity>> GetAllEntities();

        Task Add(TEntity entity);

        Task Delete(TEntity entity);

        Task Update(TEntity entity);

        Task SaveChanges();
    }
}
