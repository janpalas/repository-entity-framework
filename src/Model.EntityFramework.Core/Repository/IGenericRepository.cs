using System.Threading.Tasks;
using Pally.Model.EntityFramework.Core.Entity;

namespace Pally.Model.EntityFramework.Core.Repository
{
    public interface IGenericRepository<TEntity> where TEntity : IEntity
    {
        TEntity GetById(int id);

        Task<TEntity> GetByIdAsync(int id);

        void Insert(TEntity entity);

        void Delete(int id);

        void Delete(TEntity entity);
    }
}
