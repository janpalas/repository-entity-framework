using System;
using System.Data;
using System.Data.Common;
using System.Data.Entity.Core.Objects;
using System.Threading;
using System.Threading.Tasks;

namespace Pally.Model.EntityFramework.Core
{
    public class UnitOfWork : IUnitOfWork, IDisposable
    {
        private readonly IDbContext _dbContext;

        private DbTransaction _transaction;
        private ObjectContext _objectContext;

        public UnitOfWork(IDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public int SaveChanges()
        {
            return _dbContext.SaveChanges();
        }

        public async Task<int> SaveChangesAsync()
        {
            return await _dbContext.SaveChangesAsync();
        }

        public async Task<int> SaveChangesAsync(CancellationToken cancellationToken)
        {
            return await _dbContext.SaveChangesAsync(cancellationToken);
        }

        public void BeginTransaction(IsolationLevel isolationLevel = IsolationLevel.Unspecified)
        {
            _objectContext = _dbContext.ObjectContext;
            if (_objectContext.Connection.State != ConnectionState.Open)
            {
                _objectContext.Connection.Open();
            }

            _transaction = _objectContext.Connection.BeginTransaction(isolationLevel);
        }

        public async Task BeginTransactionAsync(IsolationLevel isolationLevel = IsolationLevel.Unspecified)
        {
            await BeginTransactionAsync(CancellationToken.None, isolationLevel);
        }

        public async Task BeginTransactionAsync(CancellationToken cancellationToken,
            IsolationLevel isolationLevel = IsolationLevel.Unspecified)
        {
            _objectContext = _dbContext.ObjectContext;
            if (_objectContext.Connection.State != ConnectionState.Open)
            {
                await _objectContext.Connection.OpenAsync(cancellationToken);
            }

            _transaction = _objectContext.Connection.BeginTransaction(isolationLevel);
        }

        public void Commit()
        {
            _dbContext.SaveChanges();
            CommitTransaction();
        }

        public async Task CommitAsync()
        {
            await _dbContext.SaveChangesAsync();
            CommitTransaction();
        }

        public void Rollback()
        {
            if (_transaction == null)
                return;

            try
            {
                _transaction.Rollback();
            }
            finally
            {
                _transaction.Dispose();
                _transaction = null;
            }
        }

        private void CommitTransaction()
        {
            if (_transaction == null)
                return;

            try
            {
                _transaction.Commit();
            }
            finally
            {
                _transaction.Dispose();
                _transaction = null;
            }
        }

        #region IDisposable

        private bool _diposed;

        public void Dispose()
        {
            if(_diposed) return;
            
            Dispose(true);
            _diposed = true;
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposing) return;

            if (_objectContext != null)
            {
                if (_objectContext.Connection.State == ConnectionState.Open)
                {
                    _objectContext.Connection.Close();
                }

                _objectContext.Dispose();
                _objectContext = null;
            }

            if (_transaction != null)
            {
                _transaction.Rollback();
                _transaction.Dispose();
                _transaction = null;
            }
        }

        #endregion
    }
}
