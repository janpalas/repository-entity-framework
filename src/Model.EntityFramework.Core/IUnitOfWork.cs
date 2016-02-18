using System.Data;
using System.Threading;
using System.Threading.Tasks;

namespace Pally.Model.EntityFramework.Core
{
    public interface IUnitOfWork
    {
        int SaveChanges();

        Task<int> SaveChangesAsync();
         
        Task<int> SaveChangesAsync(CancellationToken cancellationToken);

        void BeginTransaction(IsolationLevel isolationLevel = IsolationLevel.Unspecified);

        Task BeginTransactionAsync(IsolationLevel isolationLevel = IsolationLevel.Unspecified);

        Task BeginTransactionAsync(CancellationToken cancellationToken, IsolationLevel isolationLevel = IsolationLevel.Unspecified);

        void Commit();

        Task CommitAsync();

        void Rollback();
    }
}
