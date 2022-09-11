using Microsoft.EntityFrameworkCore;
using Portal.Domain.BaseModels;
using Portal.Domain.Interfaces;
using Portal.Domain.Specifications;
using System.Linq.Expressions;

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

        public Task<TEntity> FindById(Guid id)
        {
            return Entities.FirstOrDefaultAsync(entity => entity.Id == id);
        }

        public Task<TEntity> FindByIdWithIncludesAsync(Guid id, string[] includeNames)
        {
            if (includeNames == null)
            {
                throw new ArgumentNullException("Include names can't be null");
            }

            IQueryable<TEntity> query = Entities;
            foreach (var includeName in includeNames)
            {
                query = query.Include(includeName);
            }

            return query.FirstOrDefaultAsync(entity => entity.Id == id);
        }

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

        public Task<List<TEntity>> FindEntitiesBySpecification(Specification<TEntity> specification)
        {
            var res = Entities;
            var ent = res.FirstOrDefault(specification?.ToExpression());
            return Entities.Where(specification?.ToExpression()).ToListAsync();
        }
    }
}
