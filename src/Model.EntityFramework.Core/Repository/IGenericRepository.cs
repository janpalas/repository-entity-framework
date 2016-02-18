using System.Collections.Generic;
using System.Threading.Tasks;
using Pally.Model.EntityFramework.Core.Entity;

namespace Pally.Model.EntityFramework.Core.Repository
{
    public interface IGenericRepository<TEntity> where TEntity : class, IEntity 
    {
        TEntity GetById(int id);

        Task<TEntity> GetByIdAsync(int id);

        IList<TEntity> GetByIds(IList<int> ids);

        Task<IList<TEntity>> GetByIdsAsync(IList<int> ids);  

        void Insert(TEntity entity);

        void Delete(int id, bool force = false);

        void Delete(TEntity entity, bool force = false);
    }
}
