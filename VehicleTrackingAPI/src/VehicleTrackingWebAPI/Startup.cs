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
using VehicleTracking.Common;
using VehicleTracking.Interfaces.Repository;
using VehicleTracking.Interfaces.Services;
using VehicleTracking.Model;
using VehicleTracking.Repository;
using VehicleTracking.Services;
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

            services.AddAuthentication(options => {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;

            }).AddIdentityServerAuthentication(o => {

                o.Authority = "http://localhost:5000";
                o.ApiName = "VehicleTrackingAPI";
                o.RequireHttpsMetadata = false;
                o.SupportedTokens = SupportedTokens.Both;


                o.LegacyAudienceValidation = false;
            });

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
