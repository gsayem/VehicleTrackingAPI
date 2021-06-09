using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VehicleTracking.Common;
using VehicleTracking.IdentityServer.Model;
using VehicleTracking.IdentityServer.Repository;
using VehicleTracking.IdentityServer.Services;
using VehicleTracking.IdentityServer.Web.Configuration;
using Microsoft.EntityFrameworkCore;
using IdentityServer4.Stores;
using VehicleTracking.IdentityServer.Stores;
using VehicleTracking.Interfaces.Repository;
using VehicleTracking.Repository;
using VehicleTracking.IdentityServer.Web.API.Services;
using VehicleTracking.IdentityServer.Web.API.Helpers;
using VehicleTracking.Web.Common.Interfaces;
using VehicleTracking.IdentityServer.Web.Middleware;
using VehicleTracking.IdentityServer.Web.API.Infrastructure;
using IdentityServer4.Services;
using VehicleTracking.Web.Common;
using IdentityServer4.Validation;
using VehicleTracking.IdentityServer.Web.Extensions;
using IdentityServer4.ResponseHandling;

namespace VehicleTracking.IdentityServer.Web {
    public class Startup {
        public Startup(IConfiguration configuration) {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services) {
            services.AddControllersWithViews();

            //AddDependencies
            services.AddDbContext<IdentityServerDBContext>(options => {
                //var connectionString = Environment.GetEnvironmentVariable("VT_IDENTITY_SQL_CONNECTION");
                var connectionString = Configuration.GetConnectionString("VT_IDENTITY_SQL_CONNECTION");                
                options.UseSqlServer(connectionString).UseLazyLoadingProxies();
            });

            services.AddTransient<IDataContext>(provider => provider.GetService<IdentityServerDBContext>());

            services.AddSingleton<IExceptionResponseWriter, ExceptionResponseWriter>();
            services.AddSingleton<IExceptionHandler, ExceptionHandler>();

            services.AddTransient<IRepositoryAsync<Model.Client>, Repository<Model.Client>>();
            services.AddTransient<IRepositoryAsync<Model.ClientSecret>, Repository<Model.ClientSecret>>();
            services.AddTransient<IRepositoryAsync<Model.PersistedGrant>, Repository<Model.PersistedGrant>>();
            services.AddTransient<IRepositoryAsync<Model.ClientCorsOrigin>, Repository<Model.ClientCorsOrigin>>();
            services.AddTransient<IRepositoryAsync<Model.ApiResource>, Repository<Model.ApiResource>>();
            services.AddTransient<IRepositoryAsync<Model.IdentityResource>, Repository<Model.IdentityResource>>();
            services.AddTransient<IRepositoryAsync<Model.ApiScope>, Repository<Model.ApiScope>>();
            services.AddTransient<IRepositoryAsync<Model.PasswordHistory>, Repository<Model.PasswordHistory>>();
            services.AddTransient<IRepositoryAsync<Model.ClientClaim>, Repository<Model.ClientClaim>>();

            services.AddTransient<IRepository<Model.Client>, Repository<Model.Client>>();
            services.AddTransient<IRepository<Model.ClientSecret>, Repository<Model.ClientSecret>>();
            services.AddTransient<IRepository<Model.PersistedGrant>, Repository<Model.PersistedGrant>>();
            services.AddTransient<IRepository<Model.ClientCorsOrigin>, Repository<Model.ClientCorsOrigin>>();
            services.AddTransient<IRepository<Model.ApiResource>, Repository<Model.ApiResource>>();
            services.AddTransient<IRepository<Model.IdentityResource>, Repository<Model.IdentityResource>>();
            services.AddTransient<IRepository<Model.ApiScope>, Repository<Model.ApiScope>>();
            services.AddTransient<IRepository<Model.PasswordHistory>, Repository<Model.PasswordHistory>>();
            services.AddTransient<IRepository<Model.ClientClaim>, Repository<Model.ClientClaim>>();

            services.AddTransient<ICorsPolicyService, CorsPolicyService>();
            services.AddTransient<IClientService, ClientService>();
            services.AddTransient<IApplicationUserService, ApplicationUserService>();

            services.AddTransient<IEmailSender, AuthMessageSender>();
            services.AddTransient<ISmsSender, AuthMessageSender>();
            services.AddTransient<IUtilHelper, UtilHelper>();


            //IdentityServerConfiguration
            services.AddTransient<IClientStore, ClientStore>();
            services.AddTransient<IResourceStore, ResourceStore>();
            services.AddTransient<IPersistedGrantStore, PersistedGrantStore>();
            services.AddTransient<IUserClaimsPrincipalFactory<ApplicationUser>, UserClaimsFactory<ApplicationUser, ApplicationRole>>();
            services.AddTransient<IResourceOwnerPasswordValidator, ResourceOwnerPasswordValidator>();
            services.AddTransient<IClaimsService, ClaimsService>();
            services.AddTransient<ITokenService, TokenService>();

            services.AddTransient<IUnitOfWork, UnitOfWork>();
            
            services.AddTransient<IAuthorizeInteractionResponseGenerator, IdentityServerAuthorizeInteractionResponseGenerator>();

            services.AddCors(options => {
                options.AddPolicy("default", policy => {
                    var corsOrigins = Configuration.GetSection("CorsOrigins").Get<CorsOrigins>();
                    if (corsOrigins != null) {
                        policy.WithOrigins(corsOrigins.Origins);
                    }
                    else {
                        policy.AllowAnyOrigin();
                    }
                    policy.AllowAnyHeader().AllowAnyMethod();
                });
            });
            services.Configure<EmailSettings>(Configuration.GetSection("EmailSettings"));
            services.Configure<LockoutOptions>(Configuration.GetSection("LockoutOptions"));
            services.Configure<IdServerClientConfiguration>(Configuration.GetSection("IdServerClientConfiguration"));
            services.AddIdentity<ApplicationUser, ApplicationRole>(options => {
                options.Password = Configuration.GetSection("PasswordOptions").Get<UserPasswordOptions>();
            }).AddEntityFrameworkStores<IdentityServerDBContext>().AddDefaultTokenProviders();


            services.AddIdentityServer()
               //.AddSigningCredential(cert)
               .AddDeveloperSigningCredential()
               .AddAspNetIdentity<ApplicationUser>()
               .AddProfileService<IdentityProfileService>();

            services.AddDataMappers();
            services.AddAutoMapper(typeof(Startup));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env) {
            if (env.IsDevelopment()) {
                app.UseDeveloperExceptionPage();
            }
            else {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            //
            app.UseCors("default");
            app.UseAuthentication();
            app.UseIdentityServer();
            //

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseMiddleware<ErrorHandlingMiddleware>(app.ApplicationServices.GetService<IExceptionHandler>());
            app.UseMiddleware<Prevent302Middleware>();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints => {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
