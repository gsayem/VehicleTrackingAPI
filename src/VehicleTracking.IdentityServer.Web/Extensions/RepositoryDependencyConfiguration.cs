using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VehicleTracking.IdentityServer.Model;
using VehicleTracking.IdentityServer.Repository;
using VehicleTracking.IdentityServer.Stores;
using VehicleTracking.Interfaces.Repository;
using VehicleTracking.Repository;

namespace VehicleTracking.IdentityServer.Web.Extensions {
    public static class RepositoryDependencyConfiguration {
        public static IServiceCollection AddRepositoryDependency(this IServiceCollection services, IConfiguration configuration) {
            services.AddDbContext<IdentityServerDBContext>(options => {
                //var connectionString = Environment.GetEnvironmentVariable("VT_IDENTITY_SQL_CONNECTION");
                var connectionString = configuration.GetConnectionString("VT_IDENTITY_SQL_CONNECTION");
                options.UseSqlServer(connectionString).UseLazyLoadingProxies();
            });
            services.AddTransient<IDataContext>(provider => provider.GetService<IdentityServerDBContext>());
            services.AddTransient<IUnitOfWork, UnitOfWork>();
            //services.AddTransient(typeof(IRepository<>), typeof(Repository<>));
            //services.AddTransient(typeof(IRepositoryAsync<>), typeof(Repository<>));


            //IRepositoryAsync
            services.AddTransient<IRepositoryAsync<Client>, Repository<Client>>();
            services.AddTransient<IRepositoryAsync<ClientSecret>, Repository<ClientSecret>>();
            services.AddTransient<IRepositoryAsync<PersistedGrant>, Repository<PersistedGrant>>();
            services.AddTransient<IRepositoryAsync<ClientCorsOrigin>, Repository<ClientCorsOrigin>>();
            services.AddTransient<IRepositoryAsync<ApiResource>, Repository<ApiResource>>();
            services.AddTransient<IRepositoryAsync<IdentityResource>, Repository<IdentityResource>>();
            services.AddTransient<IRepositoryAsync<ApiScope>, Repository<ApiScope>>();
            services.AddTransient<IRepositoryAsync<ClientClaim>, Repository<ClientClaim>>();
            services.AddTransient<IRepositoryAsync<PasswordHistory>, Repository<PasswordHistory>>();

            //IRepository
            services.AddTransient<IRepository<Client>, Repository<Client>>();
            services.AddTransient<IRepository<ClientSecret>, Repository<ClientSecret>>();
            services.AddTransient<IRepository<PersistedGrant>, Repository<PersistedGrant>>();
            services.AddTransient<IRepository<ClientCorsOrigin>, Repository<ClientCorsOrigin>>();
            services.AddTransient<IRepository<ApiResource>, Repository<ApiResource>>();
            services.AddTransient<IRepository<IdentityResource>, Repository<IdentityResource>>();
            services.AddTransient<IRepository<ApiScope>, Repository<ApiScope>>();
            services.AddTransient<IRepository<ClientClaim>, Repository<ClientClaim>>();
            services.AddTransient<IRepository<PasswordHistory>, Repository<PasswordHistory>>();


            return services;
        }
    }
}
