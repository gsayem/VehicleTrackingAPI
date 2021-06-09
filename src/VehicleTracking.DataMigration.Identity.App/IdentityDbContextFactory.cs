using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using System;
using VehicleTracking.IdentityServer.Repository;
using VehicleTracking.Data.Mappings.Identity;

namespace VehicleTracking.DataMigration.Identity.App {
    public class IdentityDbContextFactory : IDesignTimeDbContextFactory<IdentityServerDBContext> {
        public IdentityServerDBContext CreateDbContext(string connectionString) {

            var builder = new DbContextOptionsBuilder<IdentityServerDBContext>().UseSqlServer(connectionString, b => b.MigrationsAssembly("VehicleTracking.DataMigration.Identity.App")).UseLazyLoadingProxies();
            builder.UseVehicleTrackingIdentityModel(connectionString);
            return new IdentityServerDBContext(builder.Options);
        }
        /// <summary>
        ///Called by dotnet ef database update.
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        public IdentityServerDBContext CreateDbContext(string[] args) {
            return CreateDbContext(Environment.GetEnvironmentVariable("VT_IDENTITY_SQL_CONNECTION"));
        }
    }
}
