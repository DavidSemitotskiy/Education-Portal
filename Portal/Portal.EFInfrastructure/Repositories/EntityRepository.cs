using Microsoft.EntityFrameworkCore;
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

        public async Task Delete(TEntity entity)
        {
            var entityToDelete = await Entities.FirstOrDefaultAsync(e => e.Id == entity.Id);
            if (entityToDelete != null)
            {
                Entities.Remove(entityToDelete);
            }
        }

        public Task<List<TEntity>> GetAllEntities()
        {
            return Entities.ToListAsync();
        }

        public async Task Update(TEntity entity)
        {
            var entityToUpdate = await Entities.FirstOrDefaultAsync(e => e.Id == entity.Id);
            if (entityToUpdate != null)
            {
                Entities.Update(entityToUpdate);
            }
        }

        public async Task SaveChanges()
        {
            await _context.SaveChangesAsync();
        }
    }
}
