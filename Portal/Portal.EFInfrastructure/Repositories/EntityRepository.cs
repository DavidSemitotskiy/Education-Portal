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

        public async Task Add(TEntity entity)
        {
            await Entities.AddAsync(entity);
        }

        public Task Delete(TEntity entity)
        {
            EntityEntry entityEntry = _context.Entry(entity);
            entityEntry.State = EntityState.Deleted;
            Entities.Remove(entity);
            return Task.CompletedTask;
        }

        public Task<List<TEntity>> GetAllEntities()
        {
            return Entities.ToListAsync();
        }

        public Task Update(TEntity entity)
        {
            EntityEntry entityEntry = _context.Entry(entity);
            entityEntry.State = EntityState.Modified;
            Entities.Update(entity);
            return Task.CompletedTask;
        }

        public Task<int> SaveChanges()
        {
            return _context.SaveChangesAsync();
        }
    }
}
