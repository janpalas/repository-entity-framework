using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Pally.Model.EntityFramework.Core.Entity;

namespace Pally.Model.EntityFramework.Core
{
    public interface IDbContext
    {
        IQueryable<TEntity> Queryable<TEntity>() where TEntity : class, IEntity;

        int SaveChanges();

        Task<int> SaveChangesAsync();

        Task<int> SaveChangesAsync(CancellationToken cancellationToken);
    }
}
