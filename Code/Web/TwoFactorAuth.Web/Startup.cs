namespace TwoFactorAuth.Web
{
    using System;
    using System.Reflection;

    using Data;
    using Data.Models;
    using Data.Repositories;
    using Data.Seeding;

    using Microsoft.AspNetCore.Authentication.Cookies;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Hosting;
    using Microsoft.OpenApi.Models;

    using Services.Mapping;
    using Services.Messaging;

    using TwoFactorAuth.Data.Common;
    using TwoFactorAuth.Data.Common.Repositories;
    using TwoFactorAuth.Services.Data.DummyAuthService;
    using TwoFactorAuth.Services.Data.SettingsService;
    using TwoFactorAuth.Web.ViewModels;

    public class Startup
    {
        private readonly IConfiguration _configuration;

        public Startup(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        // This method gets called by the runtime   Use this method to add services to the container
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<ApplicationDbContext>(
                options => options.UseSqlServer(_configuration.GetConnectionString("DefaultConnection"))
            );

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
                    options =>
                    {
                        options.Filters.Add(new AutoValidateAntiforgeryTokenAttribute());
                    }
                )
                .AddRazorRuntimeCompilation();

            services.AddRazorPages();
            services.AddDatabaseDeveloperPageExceptionFilter();

            services.AddSingleton(_configuration);

            // Data repositories
            services.AddScoped(typeof(IDeletableEntityRepository<>), typeof(EfDeletableEntityRepository<>));
            services.AddScoped(typeof(IRepository<>), typeof(EfRepository<>));
            services.AddScoped<IDbQueryRunner, DbQueryRunner>();

            // Application services
            services.AddTransient<IEmailSender, NullMessageSender>();
            services.AddTransient<ISettingsService, SettingsService>();
            services.AddTransient<IDummyTwoFactorAuthService, DummyTwoFactorAuthService>();

            services
                .AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie(
                    CookieAuthenticationDefaults.AuthenticationScheme,
                    options =>
                    {
                        options.LoginPath = "/DummyTwoFactorAuth/Login";
                        options.LogoutPath = "/Account/Logout";
                    }
                );

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
                        Email = string.Empty,
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

        // This method gets called by the runtime   Use this method to configure the HTTP request pipeline
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            AutoMapperConfig.RegisterMappings(typeof(ErrorViewModel).GetTypeInfo().Assembly);

            using (var serviceScope = app.ApplicationServices.CreateScope()) // Seed data on application startup
            {
                var dbContext = serviceScope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
                dbContext.Database.Migrate();

                new ApplicationDbContextSeeder().SeedAsync(dbContext, serviceScope.ServiceProvider).GetAwaiter().GetResult();
            }

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseMigrationsEndPoint();

                app.UseSwagger();
                app.UseSwaggerUI(
                    c => c.SwaggerEndpoint(
                        "/swagger/v1/swagger.json",
                        "WebApp1 v1"
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
    }
}
