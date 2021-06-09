using IdentityServer4.AccessTokenValidation;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using VehicleTracking.Common;
using VehicleTracking.Interfaces.Correlation;
using VehicleTracking.Interfaces.Repository;
using VehicleTracking.Interfaces.Services;
using VehicleTracking.Model;
using VehicleTracking.Repository;
using VehicleTracking.Services;
using VehicleTracking.Web.Common;
using VehicleTracking.Web.Common.Interfaces;
using VehicleTracking.Web.Common.Middleware;
using VehicleTracking.WebAPI.AuthorizationHandlers;
using VehicleTracking.WebAPI.Infrastructure;
using static VehicleTracking.WebAPI.Infrastructure.ApiConstants;

namespace VehicleTracking.WebAPI {
    public class Startup {
        public Startup(IConfiguration configuration) {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services) {
            services.AddDbContext<VehicleTrackingDBContext>(options => {
                var connectionString = Configuration.GetConnectionString("VT_API_SQL_CONNECTION");
                options.UseSqlServer(connectionString).UseLazyLoadingProxies();
            });
            services.AddControllers();
            //services.AddAuthentication("Bearer")
            //.AddJwtBearer("Bearer", options => {
            //    options.Authority = "https://localhost:5000";
                
            //    options.TokenValidationParameters = new TokenValidationParameters {
            //        ValidateAudience = false
                   
            //    };
            //});
            //.AddIdentityServerAuthentication(o => {

            //    o.Authority = "http://localhost:5000";

            //    o.ApiName = "VehicleTrackingAPI";
            //    o.RequireHttpsMetadata = false;
            //    o.SupportedTokens = SupportedTokens.Both;
            //}); ;

            services.AddSwaggerGen(c => {
                var jwtSecurityScheme = new OpenApiSecurityScheme {
                    Scheme = "bearer",
                    BearerFormat = "JWT",
                    Name = "JWT Authentication",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.Http,
                    Description = "Put **_ONLY_** your JWT Bearer token on textbox below!",

                    Reference = new OpenApiReference {
                        Id = JwtBearerDefaults.AuthenticationScheme,
                        Type = ReferenceType.SecurityScheme
                    }
                };

                c.AddSecurityDefinition(jwtSecurityScheme.Reference.Id, jwtSecurityScheme);
                c.AddSecurityRequirement(new OpenApiSecurityRequirement { { jwtSecurityScheme, Array.Empty<string>() } });
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "VehicleTracking.WebAPI", Version = "v1" });
            });




            services.AddCors(options => {
                options.AddPolicy("default", policy => {
                    var corsOrigins = Configuration.GetSection("CorsOrigins").Get<CorsOrigins>();
                    if (corsOrigins != null) {
                        policy.WithOrigins(corsOrigins.Origins);
                    } else {
                        policy.AllowAnyOrigin();
                    }
                    policy.AllowAnyHeader().AllowAnyMethod();
                });
            });
            var key = Encoding.ASCII.GetBytes("VcSwJ0LIJS3Ye158loFI68mMpZs3/LitLC77gaHwKys=");
            //var cert = new X509Certificate2(key);
            var keyBase64String = Convert.ToBase64String(key);
            var store = new X509Store(StoreName.My, StoreLocation.LocalMachine);
            store.Open(OpenFlags.ReadOnly);
            //var certCollection = store.Certificates.Find(X509FindType.FindByThumbprint, keyBase64String, false);
            var cert = store.Certificates[0];

            SecurityKey securityKey = new X509SecurityKey(cert);
            services.AddAuthentication(options => {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;

            }).AddJwtBearer(x =>
            {
                x.RequireHttpsMetadata = false;
                x.SaveToken = true;
                x.TokenValidationParameters = new TokenValidationParameters {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = securityKey,// new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false
                };
            });

            //    .AddIdentityServerAuthentication(o => {

            //    o.Authority = "http://localhost:5000";

            //    o.ApiName = "VehicleTrackingAPI";
            //    o.RequireHttpsMetadata = false;
            //    o.SupportedTokens = SupportedTokens.Both;
            //});

            services.AddAuthorization(options => {
                options.AddPolicy(ApiConstants.AuthorizationPolicy.MustBeAdmin,
                    policy => policy.Requirements.Add(new AppTypeRequirement(ClaimType.AppType.Admin)));

                options.AddPolicy(ApiConstants.AuthorizationPolicy.MustBeAdmin,
                    policy => policy.Requirements.Add(new AppTypeRequirement(ClaimType.AppType.VTUser)));
            });

            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddSingleton<IActionContextAccessor, ActionContextAccessor>();
            services.AddTransient<IDataContext, VehicleTrackingDBContext>();
            services.AddTransient<IUnitOfWork, UnitOfWork>();
            services.AddTransient(typeof(IRepository<>), typeof(Repository<>));
            services.AddTransient(typeof(IRepositoryAsync<>), typeof(Repository<>));

            services.AddTransient<IRepository<Vehicle>, Repository<Vehicle>>();
            services.AddTransient<IRepository<VehiclePosition>, Repository<VehiclePosition>>();
            services.AddTransient<IRepositoryAsync<Vehicle>, Repository<Vehicle>>();
            services.AddTransient<IRepositoryAsync<VehiclePosition>, Repository<VehiclePosition>>();

            services.AddTransient<IVehicleService, VehicleService>();
            services.AddTransient<IVehiclePositionService, VehiclePositionService>();

            services.AddMvc(config => {
                var policy = new AuthorizationPolicyBuilder()
                    .RequireAuthenticatedUser()
                    .Build();
                config.Filters.Add(new AuthorizeFilter(policy));
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env) {
            if (env.IsDevelopment()) {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "VehicleTracking.WebAPI v1"));
            }

            //
            app.UseCors("default");
            app.UseAuthentication();

            ServiceFactory.AddServiceProvider(app.ApplicationServices);

            //app.UseMiddleware<CorrelationIdMiddleware>(app.ApplicationServices.GetService<ICorrelationService>());
            //app.UseMiddleware<ErrorHandlingMiddleware>(app.ApplicationServices.GetService<IExceptionHandler>());
            //app.UseMiddleware<Prevent302Middleware>();
            //


            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints => {
                endpoints.MapControllers();
            });
        }
    }
}
