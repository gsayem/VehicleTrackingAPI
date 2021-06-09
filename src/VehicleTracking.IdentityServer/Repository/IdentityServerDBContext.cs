
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using VehicleTracking.IdentityServer.Model;
using VehicleTracking.Interfaces.Repository;
using VehicleTracking.Model;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace VehicleTracking.IdentityServer.Repository {
    public class IdentityServerDBContext : IdentityDbContext<ApplicationUser, ApplicationRole, Guid>, IDataContext {
        public static void ConsoleLog(string message) {
            if (!String.IsNullOrWhiteSpace(message)) {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine(message);
                Console.ForegroundColor = ConsoleColor.Gray;
            }
        }
        protected string ConnectionString { private set; get; }
        private readonly bool _optionsConfigured = false;
        public IdentityServerDBContext(string connectionString) {
            ConnectionString = connectionString;
        }
        public IdentityServerDBContext(DbContextOptions<IdentityServerDBContext> options) : base(options) {
            _optionsConfigured = true;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) {
            if (string.IsNullOrWhiteSpace(ConnectionString)) {
                foreach (var item in optionsBuilder.Options.Extensions) {
#pragma warning disable EF1001 // Internal EF Core API usage.
                    if (item.GetType() == typeof(Microsoft.EntityFrameworkCore.SqlServer.Infrastructure.Internal.SqlServerOptionsExtension)) {
#pragma warning restore EF1001 // Internal EF Core API usage.
                        ConnectionString = ((Microsoft.EntityFrameworkCore.SqlServer.Infrastructure.Internal.SqlServerOptionsExtension)item).ConnectionString;
                        break;
                    }
                }
            }
            if (!string.IsNullOrWhiteSpace(ConnectionString) && !_optionsConfigured)
            {
                optionsBuilder.UseSqlServer(ConnectionString, b => b.MigrationsAssembly("VehicleTracking.DataMigrator.Identity.App")).UseLazyLoadingProxies();
            }
            base.OnConfiguring(optionsBuilder);
        }
        private void WriteAuditData() {
            foreach (var entry in ChangeTracker.Entries()) {
                if (entry.State == EntityState.Unchanged) {
                    continue;
                }

                BaseModel entity = entry.Entity as BaseModel;

                if (entity == null) {
                    continue;
                }

                if (entry.State == EntityState.Deleted) {
                    entity.OnDelete();
                }

                if (entry.State == EntityState.Added) {
                    entity.OnCreate();
                }

                if (entry.State == EntityState.Modified) {
                    entity.OnUpdate();
                }
            }
        }
        public IEnumerable<EntityEntry> GetChangeTrackerEntries() {
            return ChangeTracker.Entries();
        }
        public override int SaveChanges() {
            WriteAuditData();

            return base.SaveChanges();
        }
        public Task<int> SaveChangesAsync() {
            WriteAuditData();
            return SaveChangesAsync(new CancellationToken());
        }

        public DbSet<ApiResource> ApiResources { get; set; }
        public DbSet<ApiResourceClaim> ApiResourceClaims { get; set; }
        public DbSet<ApiScope> ApiScopes { get; set; }
        public DbSet<ApiScopeClaim> ApiScopeClaims { get; set; }
        public DbSet<ApiSecret> ApiSecrets { get; set; }
        public DbSet<Client> Clients { get; set; }
        public DbSet<ClientClaim> ClientClaims { get; set; }
        public DbSet<ClientCorsOrigin> ClientCorsOrigins { get; set; }
        public DbSet<ClientGrantType> ClientGrantTypes { get; set; }
        public DbSet<ClientIdPRestriction> ClientIdPRestrictions { get; set; }
        public DbSet<ClientPostLogoutRedirectUri> ClientPostLogoutRedirectUris { get; set; }
        public DbSet<ClientRedirectUri> ClientRedirectUris { get; set; }
        public DbSet<ClientScope> ClientScopes { get; set; }
        public DbSet<ClientSecret> ClientSecrets { get; set; }
        public DbSet<IdentityClaim> IdentityClaims { get; set; }
        public DbSet<IdentityResource> IdentityResources { get; set; }
        public DbSet<PasswordHistory> PasswordHistories { get; set; }
        public DbSet<PersistedGrant> PersistedGrants { get; set; }
        //public DbSet<Model.UserClaim> UserClaims { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder) {
            modelBuilder.Ignore<BaseModel>();
            //foreach (var entity in modelBuilder.Model.GetEntityTypes())
            //{
            //    ConsoleLog(entity.ClrType.Name);
            //    entity.Relational().TableName = entity.ClrType.Name;
            //}
            base.OnModelCreating(modelBuilder);
        }

        public string ConString() {
            return ConnectionString;
        }
    }
}
