using AutoMapper;
using Microsoft.Extensions.DependencyInjection;
using VehicleTracking.IdentityServer.Model;
using VehicleTracking.IdentityServer.Web.ViewModels;

namespace VehicleTracking.IdentityServer.Web.Extensions {
    public static class MappersConfiguration
    {
        public static IServiceCollection AddDataMappers(this IServiceCollection services)
        {
            var configuration = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Client, ClientViewModel>();
                cfg.CreateMap<ClientViewModel, Client>();
                cfg.CreateMap<ClientGrantType, ClientGrantTypeViewModel>();
                cfg.CreateMap<ClientGrantTypeViewModel, ClientGrantType>();
                cfg.CreateMap<ClientScope, ClientScopeViewModel>();
                cfg.CreateMap<ClientScopeViewModel, ClientScope>();
                cfg.CreateMap<ClientRedirectUriViewModel, ClientRedirectUri>();
                cfg.CreateMap<ClientRedirectUri, ClientRedirectUriViewModel>();
                cfg.CreateMap<ClientPostLogoutRedirectUriViewModel, ClientPostLogoutRedirectUri>();
                cfg.CreateMap<ClientPostLogoutRedirectUri, ClientPostLogoutRedirectUriViewModel>();

            });

            //configuration.CompileMappings();
            services.AddSingleton(configuration.CreateMapper());
            return services;
        }
    }
}
