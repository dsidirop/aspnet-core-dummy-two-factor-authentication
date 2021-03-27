// ReSharper disable CA1822

namespace TwoFactorAuth.Web
{
    using System;
    using Autofac;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Hosting;
    using Microsoft.OpenApi.Models;
    using TwoFactorAuth.Data;
    using TwoFactorAuth.Data.Models;
    using TwoFactorAuth.Web.IoC;
    using TwoFactorAuth.Web.StartupX;

    public class Startup
    {
        private readonly IConfiguration _configuration;

        public Startup(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.SetDefaultThreadCulture();
            app.RegisterMappings();
            app.RunDbMigrationsAndSeeders();
            app.ScaffoldImages();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseMigrationsEndPoint();

                app.UseSwagger();
                app.UseSwaggerUI(
                    c => c.SwaggerEndpoint(
                        url: "/swagger/v1/swagger.json",
                        name: "WebApp1 v1"
                    )
                );
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts(); // http strict transport security header
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseCookiePolicy();

            app.UseRouting();

            app.UseAuthentication(); //0
            app.UseAuthorization(); //0

            app.UseEndpoints(
                endpoints =>
                {
                    endpoints.MapControllerRoute("areaRoute", "{area:exists}/{controller=Home}/{action=Index}/{id?}"); //   order
                    endpoints.MapControllerRoute("default", "{controller=Home}/{action=Index}/{id?}"); //                   order
                    endpoints.MapRazorPages();
                }
            );

            //0 https://docs.microsoft.com/en-us/aspnet/core/security/authentication/scaffold-identity?view=aspnetcore-5.0&tabs=visual-studio
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<ApplicationDbContext>(
                options =>
                {
                    var connectionString = _configuration.GetConnectionString("DefaultConnection");

                    options.UseSqlServer(connectionString);
                });

            services
                .AddDefaultIdentity<ApplicationUser>(IdentityOptionsProvider.GetIdentityOptions)
                .AddRoles<ApplicationRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>();

            services.Configure<CookiePolicyOptions>(
                options =>
                {
                    options.CheckConsentNeeded = _ => true;
                    options.MinimumSameSitePolicy = SameSiteMode.None;
                }
            );

            services
                .AddControllersWithViews(
                    options => { options.Filters.Add(new AutoValidateAntiforgeryTokenAttribute()); }
                )
                .AddRazorRuntimeCompilation();

            services.AddRazorPages();
            services.AddDatabaseDeveloperPageExceptionFilter();

            services.AddConfigurationSections(_configuration);

            services.ConfigureApplicationCookie(options => options.LoginPath = "/Identity/Login"); //vital

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Version = "v1",
                    Title = "TwoFactorAuth API",
                    Description = "TwoFactorAuth ASP.NET Core Web API",
                    TermsOfService = new Uri("https://acmecorp.com/terms"),
                    Contact = new OpenApiContact
                    {
                        Name = "Acme Corp.",
                        Email = "corp@acme.com",
                        Url = new Uri("https://acmecorp.com/about"),
                    },
                    License = new OpenApiLicense
                    {
                        Name = "GPL v3",
                        Url = new Uri("https://acmecorp.com/license"),
                    },
                });
            });
        }

        public void ConfigureContainer(ContainerBuilder builder)
        {
            builder
                .AddAutofaqDiagnostics() //order
                .RegisterModule(new AutofacModule(_configuration)); //order
        }
    }
}
