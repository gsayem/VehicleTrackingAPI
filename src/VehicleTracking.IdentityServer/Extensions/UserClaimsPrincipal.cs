using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using VehicleTracking.IdentityServer.Model;

namespace VehicleTracking.IdentityServer.Extensions
{
    public static class UserClaimsPrincipal
    {
        public static IServiceCollection AddIdentityServerUserClaimsPrincipalFactory(this IServiceCollection services)
        {
            services.TryAddTransient<IUserClaimsPrincipalFactory<ApplicationUser>, UserClaimsFactory<ApplicationUser, ApplicationRole>>();            
            //services.TryAddTransient<IUserClaimsPrincipalFactory<ApplicationUser>, UserClaimsPrincipalFactory<ApplicationUser, ApplicationRole>>();

            return services;
        }
    }
}

