using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using VehicleTracking.Common;

namespace VehicleTracking.IdentityServer.Web.Extensions {
    public static class CorsConfiguration {
        public static IServiceCollection AddCors(this IServiceCollection services, IConfiguration configuration) {
            services.AddCors(options => {
                options.AddPolicy("default", policy => {
                    var corsOrigins = configuration.GetSection("CorsOrigins").Get<CorsOrigins>();
                    if (corsOrigins != null) {
                        policy.WithOrigins(corsOrigins.Origins);
                    } else {
                        policy.AllowAnyOrigin();
                    }
                    policy.AllowAnyHeader().AllowAnyMethod();
                });
            });
            return services;
        }
    }
}
