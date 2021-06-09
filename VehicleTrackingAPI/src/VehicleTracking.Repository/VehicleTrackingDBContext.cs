using VehicleTracking.Interfaces.Repository;
using VehicleTracking.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace VehicleTracking.Repository
{
    public partial class VehicleTrackingDBContext : DbContext, IDataContext
    {
        protected string ConnectionString { private set; get; }
        private readonly bool _optionsConfigured = false;
        public VehicleTrackingDBContext()
        {
        }

        public VehicleTrackingDBContext(string connectionString)
        {
            ConnectionString = connectionString;
        }

        public VehicleTrackingDBContext(DbContextOptions<VehicleTrackingDBContext> options) : base(options)
        {
            _optionsConfigured = true;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (string.IsNullOrWhiteSpace(ConnectionString))
            {
                foreach (var item in optionsBuilder.Options.Extensions)
                {
                    #pragma warning disable EF1001 // Internal EF Core API usage.
                    if (item.GetType() == typeof(Microsoft.EntityFrameworkCore.SqlServer.Infrastructure.Internal.SqlServerOptionsExtension))
                    #pragma warning restore EF1001 // Internal EF Core API usage.
                    {
                        ConnectionString = ((Microsoft.EntityFrameworkCore.SqlServer.Infrastructure.Internal.SqlServerOptionsExtension)item).ConnectionString;
                        break;
                    }
                }
            }
            if (!string.IsNullOrWhiteSpace(ConnectionString) && !_optionsConfigured)
            {
                optionsBuilder.UseSqlServer(ConnectionString, b => b.MigrationsAssembly("VehicleTracking.DataMigration.App")).UseLazyLoadingProxies();
            }

            base.OnConfiguring(optionsBuilder);

        }

        public override DbSet<TEntity> Set<TEntity>()
        {
            return base.Set<TEntity>();
        }

        public override EntityEntry<TEntity> Entry<TEntity>(TEntity entity)
        {
            return base.Entry<TEntity>(entity);
        }

        public override EntityEntry Entry(object entity)
        {
            return base.Entry(entity);
        }

        public IEnumerable<EntityEntry> GetChangeTrackerEntries()
        {
            return ChangeTracker.Entries();
        }

        private string GetDbConnectionString()
        {
            var dbName = "In Memory ?";

            try
            {
                //dbName = base.Database.GetDbConnection().ConnectionString;
            }
            catch
            {
                // dont care about the exception here, just want the connection string or "In Memory ?"
            }

            return dbName;
        }

        public override int SaveChanges()
        {
            WriteAuditData();

            return base.SaveChanges();
        }

        private void WriteAuditData()
        {
            foreach (var entry in ChangeTracker.Entries())
            {
                if (entry.State == EntityState.Unchanged)
                {
                    continue;
                }

                BaseModel entity = entry.Entity as BaseModel;

                if (entity == null)
                {
                    continue;
                }

                if (entry.State == EntityState.Deleted)
                {
                    entity.OnDelete();
                }

                if (entry.State == EntityState.Added)
                {
                    entity.OnCreate();
                }

                if (entry.State == EntityState.Modified)
                {
                    entity.OnUpdate();
                }
            }
        }

        public async Task<int> SaveChangesAsync()
        {
            WriteAuditData();
            return await base.SaveChangesAsync();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }
}
