using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using Pally.Model.EntityFramework.Core.Entity;

namespace Pally.Model.EntityFramework.Core.Repository
{
    public class GenericRepository<TEntity> : IGenericRepository<TEntity> where TEntity : class, IEntity
    {
        protected readonly DbSet<TEntity> DbSet; 

        public GenericRepository(IDbContext dbContext)
        {
            DbSet = dbContext.Set<TEntity>();
        }

        public virtual TEntity GetById(int id)
        {
            return DbSet.Find(id);
        }

        public virtual async Task<TEntity> GetByIdAsync(int id)
        {
            return await DbSet.FindAsync(id);
        }

        public IList<TEntity> GetByIds(IList<int> ids)
        {
            if (ids == null || !ids.Any())
                return Enumerable.Empty<TEntity>().ToList();

            return DbSet.Where(x => ids.Contains(x.Id)).ToList();
        }

        public async Task<IList<TEntity>> GetByIdsAsync(IList<int> ids)
        {
            if (ids == null || !ids.Any())
                return Enumerable.Empty<TEntity>().ToList();

            return await DbSet.Where(x => ids.Contains(x.Id)).ToListAsync();
        }

        public virtual void Insert(TEntity entity)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity), "Entity cannot be null!");

            DbSet.Add(entity);
        }

        public virtual void Delete(int id, bool force = false)
        {
            TEntity entity = GetById(id);
            if (entity != null)
            {
                Delete(entity, force);
            }
        }

        public virtual void Delete(TEntity entity, bool force = false)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity), "Entity cannot be null!");

            var deletableEntity = entity as ILightDeletableEntity;
            if (deletableEntity == null || force)
            {
                DbSet.Remove(entity);
            }
            else
            {
                deletableEntity.IsDeleted = true;
            }
        }
    }
}
