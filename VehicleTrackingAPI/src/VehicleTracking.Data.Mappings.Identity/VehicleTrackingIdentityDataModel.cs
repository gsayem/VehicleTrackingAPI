using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Generic;
using VehicleTracking.Data.Mappings.Identity.Mappings;

namespace VehicleTracking.Data.Mappings.Identity {
    public static class VehicleTrackingIdentityDataModel {
        private static ConventionSet Build(string connectionString) {

            var serviceProvider = new ServiceCollection()
                .AddEntityFrameworkSqlServer().AddDbContext<DbContext>(o => o.UseSqlServer(connectionString)).BuildServiceProvider();

            using var serviceScope = serviceProvider.GetRequiredService<IServiceScopeFactory>().CreateScope();
            using var context = serviceScope.ServiceProvider.GetService<DbContext>();
            return ConventionSet.CreateConventionSet(context);

        }
        public static DbContextOptionsBuilder UseVehicleTrackingIdentityModel(this DbContextOptionsBuilder dbContextBuilder, string connectionString) {
            var cs = Build(connectionString);
            var builder = new ModelBuilder(cs);
            var mappings = RegisterMappers();
            foreach (var entityMapper in mappings) {
                entityMapper.MapEntity(builder);
            }
            dbContextBuilder.UseModel(builder.FinalizeModel());
            return dbContextBuilder;
        }

        private static IList<IBaseModelMapper> RegisterMappers() {
            return new List<IBaseModelMapper>()
            {
new AspNetContextMapping(),
                new ClientContextMappings(),
                new PersistedGrantContextMappings(),
                new ResourcesContextMappings()
            };
        }
    }
}
