using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Portal.Domain.BaseModels;
using Portal.Domain.Interfaces;

namespace Portal.EFInfrastructure.Repositories
{
    public class EntityRepository<TEntity> : IEntityRepository<TEntity> where TEntity : Entity
    {
        private Context _context;

        public EntityRepository(Context context)
        {
            _context = context ?? throw new ArgumentNullException("Context can't be null");
            Entities = _context.Set<TEntity>();
        }

        public DbSet<TEntity> Entities { get; set; }

        public Task Add(TEntity entity)
        {
            Entities.Add(entity);
            return Task.CompletedTask;
        }

        public void Delete(TEntity entity)
        {
            Entities.Remove(entity);
        }

        public Task<List<TEntity>> GetAllEntities()
        {
            return Entities.ToListAsync();
        }

        public void Update(TEntity entity)
        {
            Entities.Update(entity);
        }

        public Task<int> SaveChanges()
        {
            return _context.SaveChangesAsync();
        }
    }
}
