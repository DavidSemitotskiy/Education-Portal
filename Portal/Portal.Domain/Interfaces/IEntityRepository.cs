using Portal.Domain.BaseModels;

namespace Portal.Domain.Interfaces
{
    public interface IEntityRepository<TEntity> where TEntity : Entity
    {
        Task<List<TEntity>> GetAllEntities();

        Task Add(TEntity entity);

        void Delete(TEntity entity);

        void Update(TEntity entity);

        Task<int> SaveChanges();
    }
}
