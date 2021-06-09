using VehicleTracking.Repository;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using System;
using System.Collections.Generic;
using System.Text;
using VehicleTracking.Data.Mappings;

namespace VehicleTracking.DataMigration.App
{
    public class VehicleTrackingDBContextFactory : IDesignTimeDbContextFactory<VehicleTrackingDBContext>
    {
        public VehicleTrackingDBContext CreateDbContext(string connectionString)
        {
            var builder = new DbContextOptionsBuilder<VehicleTrackingDBContext>().UseSqlServer(connectionString, b => b.MigrationsAssembly("VehicleTracking.DataMigration.App")).UseLazyLoadingProxies();
            builder.UseCalCentreModel(connectionString);
            return new VehicleTrackingDBContext(builder.Options);
        }
        /// <summary>
        ///Called by dotnet ef database update.
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        public VehicleTrackingDBContext CreateDbContext(string[] args)
        {
            Console.WriteLine(Environment.GetEnvironmentVariable("VT_API_SQL_CONNECTION"));
            return CreateDbContext(Environment.GetEnvironmentVariable("VT_API_SQL_CONNECTION"));
        }
    }
}
