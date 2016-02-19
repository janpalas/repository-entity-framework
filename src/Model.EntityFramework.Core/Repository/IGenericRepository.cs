using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Pally.Model.EntityFramework.Core.Entity;

namespace Pally.Model.EntityFramework.Core.Repository
{
    public interface IGenericRepository<TEntity> where TEntity : class, IEntity 
    {
        TEntity GetById(int id);
        TEntity GetById(int id, params Expression<Func<TEntity, object>>[] includes);

        Task<TEntity> GetByIdAsync(int id);
        Task<TEntity> GetByIdAsync(int id, params Expression<Func<TEntity, object>>[] includes);


        IList<TEntity> GetByIds(IList<int> ids);
        Task<IList<TEntity>> GetByIdsAsync(IList<int> ids);  

        void Insert(TEntity entity);
        void Delete(int id, bool force = false);
        void Delete(TEntity entity, bool force = false);
    }
}
