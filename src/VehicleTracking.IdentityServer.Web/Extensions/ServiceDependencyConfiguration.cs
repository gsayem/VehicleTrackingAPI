using IdentityServer4.EntityFramework.Stores;
using IdentityServer4.ResponseHandling;
using IdentityServer4.Services;
using IdentityServer4.Stores;
using IdentityServer4.Validation;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using VehicleTracking.IdentityServer.Extensions;
using VehicleTracking.IdentityServer.Model;
using VehicleTracking.IdentityServer.Services;
using VehicleTracking.IdentityServer.Web.API.Helpers;
using VehicleTracking.IdentityServer.Web.API.Infrastructure;
using VehicleTracking.IdentityServer.Web.API.Services;
using VehicleTracking.Web.Common.Interfaces;
using VehicleTracking.Web.Common.Middleware;

namespace VehicleTracking.IdentityServer.Web.Extensions {
    public static class ServiceDependencyConfiguration {


        public static IServiceCollection AddServiceDependency(this IServiceCollection services, IConfiguration configuration) {

            services.AddSingleton<IExceptionResponseWriter, ExceptionResponseWriter>();
            services.AddSingleton<IExceptionHandler, ExceptionHandler>();



            services.AddTransient<ICorsPolicyService, CorsPolicyService>();
            services.AddTransient<IClientService, ClientService>();
            services.AddTransient<IApplicationUserService, ApplicationUserService>();

            services.AddTransient<IEmailSender, AuthMessageSender>();
            services.AddTransient<ISmsSender, AuthMessageSender>();
            services.AddTransient<IUtilHelper, UtilHelper>();

            //services.AddTransient<IdentityServer4.EntityFramework.Stores.ClientStore>, Repository<IdentityServer4.EntityFramework.Stores.ClientStore>>();

            //IdentityServerConfiguration
            services.AddTransient<IClientStore, Stores.ClientStore>();
            services.AddTransient<IResourceStore, Stores.ResourceStore>();
            services.AddTransient<IPersistedGrantStore, Stores.PersistedGrantStore>();

            //Need to check
            services.AddIdentityServerUserClaimsPrincipalFactory();
            services.AddTransient<IUserClaimsPrincipalFactory<ApplicationUser>, UserClaimsFactory<ApplicationUser, ApplicationRole>>();
            
            services.AddTransient<IResourceOwnerPasswordValidator, ResourceOwnerPasswordValidator>();
            services.AddTransient<IClaimsService, ClaimsService>();
            services.AddTransient<ITokenService, TokenService>();

            services.AddTransient<IAuthorizeInteractionResponseGenerator, IdentityServerAuthorizeInteractionResponseGenerator>();
            return services;

        }
    }
}
