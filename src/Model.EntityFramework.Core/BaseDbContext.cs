using System;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Globalization;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Threading;
using System.Threading.Tasks;
using Pally.Model.EntityFramework.Core.Entity;

namespace Pally.Model.EntityFramework.Core
{
    public class BaseDbContext : DbContext, IDbContext
    {
        protected BaseDbContext(string nameOrConnectionString)
            :base(nameOrConnectionString)
        {
        }

        protected BaseDbContext()
        {
        }


        DbSet<TEntity> IDbContext.Set<TEntity>()
        {
            return base.Set<TEntity>();
        }

        #region Overrides

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Types<IConcurrentEntity>()
                .Configure(x => x.Property(entity => entity.RowVersion).IsConcurrencyToken());

            base.OnModelCreating(modelBuilder);
        }

        public override int SaveChanges()
        {
            SetRecordFields();
            IncreaseRowVersion();

            return base.SaveChanges();
        }

        public override async Task<int> SaveChangesAsync()
        {
            return await SaveChangesAsync(CancellationToken.None);
        }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken)
        {
            SetRecordFields();
            IncreaseRowVersion();

            return await base.SaveChangesAsync(cancellationToken);
        }

        #endregion

        private void IncreaseRowVersion()
        {
            foreach (DbEntityEntry dbEntityEntry in ChangeTracker.Entries().Where(x => x.State == EntityState.Added || x.State == EntityState.Modified))
            {
                var entity = dbEntityEntry.Entity as IConcurrentEntity;
                if (entity != null)
                {
                    entity.RowVersion = dbEntityEntry.State == EntityState.Added ? 0 : entity.RowVersion + 1;
                }
            }
        }

        private void SetRecordFields()
        {
            foreach (DbEntityEntry entityEntry in ChangeTracker.Entries().Where(x => x.State == EntityState.Added))
            {
                var recordBaseEntity = entityEntry.Entity as IRecordBaseEntity;
                if (recordBaseEntity != null)
                {
                    recordBaseEntity.Created = DateTime.Now;

                    //do not override values set in application
                    if (recordBaseEntity.CreatorId.Equals(default(int)))
                        recordBaseEntity.CreatorId = GetUserId(Thread.CurrentPrincipal.Identity) ?? default(int);
                }
            }

            foreach (DbEntityEntry entityEntry in ChangeTracker.Entries().Where(x => x.State == EntityState.Modified))
            {
                var recordBaseEntity = entityEntry.Entity as IRecordBaseEntity;
                if (recordBaseEntity != null)
                {
                    recordBaseEntity.Edited = DateTime.Now;

                    int? editorId = GetUserId(Thread.CurrentPrincipal.Identity);
                    if (editorId.HasValue)
                    {
                        recordBaseEntity.EditorId = editorId.Value;
                    }
                }
            }
        }

        private int? GetUserId(IIdentity identity)
        {
            var claimsIdentity = identity as ClaimsIdentity;
            Claim idClaim = claimsIdentity?.FindFirst(ClaimTypes.NameIdentifier);

            if (idClaim?.Value != null)
                return Convert.ToInt32(idClaim.Value, CultureInfo.InvariantCulture);

            return null;
        }
    }
}
