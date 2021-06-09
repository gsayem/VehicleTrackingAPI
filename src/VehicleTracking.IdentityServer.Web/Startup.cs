using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using VehicleTracking.IdentityServer.Model;
using VehicleTracking.IdentityServer.Repository;
using VehicleTracking.IdentityServer.Services;
using VehicleTracking.IdentityServer.Web.Configuration;
using VehicleTracking.IdentityServer.Web.Extensions;
using VehicleTracking.Interfaces.Correlation;
using VehicleTracking.Web.Common;
using VehicleTracking.Web.Common.Interfaces;
using VehicleTracking.Web.Common.Middleware;

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

            services.AddServiceDependency(Configuration);
            services.AddRepositoryDependency(Configuration);
            
            
            services.AddCors(Configuration);
            services.Configure<EmailSettings>(Configuration.GetSection("EmailSettings"));
            services.Configure<LockoutOptions>(Configuration.GetSection("LockoutOptions"));
            services.Configure<IdServerClientConfiguration>(Configuration.GetSection("IdServerClientConfiguration"));
            services.AddIdentity<ApplicationUser, ApplicationRole>(options => {
                options.Password = Configuration.GetSection("PasswordOptions").Get<UserPasswordOptions>();
            }).AddEntityFrameworkStores<IdentityServerDBContext>().AddDefaultTokenProviders();
            
            var key = Encoding.ASCII.GetBytes("VcSwJ0LIJS3Ye158loFI68mMpZs3/LitLC77gaHwKys=");
            //var cert = new X509Certificate2(key);
            var keyBase64String = Convert.ToBase64String(key);
            var store = new X509Store(StoreName.My, StoreLocation.LocalMachine);
            store.Open(OpenFlags.ReadOnly);
            //var certCollection = store.Certificates.Find(X509FindType.FindByThumbprint, keyBase64String, false);
            var cert = store.Certificates[0];

            services.AddIdentityServer()
               .AddSigningCredential(cert)
               //.AddDeveloperSigningCredential()
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

            ServiceFactory.AddServiceProvider(app.ApplicationServices);

            //app.UseMiddleware<CorrelationIdMiddleware>(app.ApplicationServices.GetService<ICorrelationService>());
            //app.UseMiddleware<ErrorHandlingMiddleware>(app.ApplicationServices.GetService<IExceptionHandler>());
            //app.UseMiddleware<Prevent302Middleware>();

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
