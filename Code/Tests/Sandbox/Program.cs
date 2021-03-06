﻿namespace Sandbox
{
    using System;
    using System.Diagnostics;
    using System.IO;
    using System.Threading.Tasks;
    using CommandLine;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Logging;
    using TwoFactorAuth.Data;
    using TwoFactorAuth.Data.Common;
    using TwoFactorAuth.Data.Common.Repositories;
    using TwoFactorAuth.Data.Models;
    using TwoFactorAuth.Data.Repositories;
    using TwoFactorAuth.Data.Seeding;
    using TwoFactorAuth.Services.Auth.Contracts;
    using TwoFactorAuth.Services.Auth.DummyAuth;
    using TwoFactorAuth.Services.Data.Contracts;
    using TwoFactorAuth.Services.Data.Settings;
    using TwoFactorAuth.Services.Messaging;
    using TwoFactorAuth.Services.Messaging.Contracts;

    static public class Program
    {
        static public int Main(string[] args)
        {
            Console.WriteLine($"{typeof(Program).Namespace} ({string.Join(" ", args)}) starts working...");

            var serviceCollection = new ServiceCollection();
            ConfigureServices(serviceCollection);

            var serviceProvider = (IServiceProvider) serviceCollection.BuildServiceProvider(true);

            // Seed data on application startup
            using (var serviceScope = serviceProvider.CreateScope())
            {
                var dbContext = serviceScope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
                dbContext.Database.Migrate();
                new ApplicationDbContextSeeder().SeedAsync(dbContext, serviceScope.ServiceProvider).GetAwaiter().GetResult();
            }

            using (var serviceScope = serviceProvider.CreateScope())
            {
                serviceProvider = serviceScope.ServiceProvider;

                return Parser.Default.ParseArguments<SandboxOptions>(args).MapResult(
                    opts => SandboxCode(opts, serviceProvider).GetAwaiter().GetResult(),
                    _ => 255
                );
            }
        }

        // ReSharper disable once UnusedParameter.Local
        static private async Task<int> SandboxCode(SandboxOptions options, IServiceProvider serviceProvider)
        {
            var sw = Stopwatch.StartNew();

            var settingsService = serviceProvider.GetRequiredService<ISettingsService>();
            Console.WriteLine($"Count of settings: {settingsService?.GetCount()}");

            Console.WriteLine(sw.Elapsed);
            return await Task.FromResult(0);
        }

        static private void ConfigureServices(IServiceCollection services)
        {
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile(
                    path: "appsettings.json",
                    optional: false,
                    reloadOnChange: true
                )
                .AddEnvironmentVariables()
                .Build();

            services.AddSingleton<IConfiguration>(configuration);

            services.AddDbContext<ApplicationDbContext>(
                options => options
                    .UseSqlServer(configuration.GetConnectionString("DefaultConnection"))
                    .UseLoggerFactory(new LoggerFactory())
            );

            services
                .AddDefaultIdentity<ApplicationUser>(IdentityOptionsProvider.GetIdentityOptions)
                .AddRoles<ApplicationRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>();

            services.AddScoped(typeof(IDeletableEntityRepository<>), typeof(EfDeletableEntityRepository<>));
            services.AddScoped(typeof(IRepository<>), typeof(EfRepository<>));
            services.AddScoped<IDbQueryRunner, DbQueryRunner>();

            // Application services
            services.AddTransient<IAppEmailSender, NullMessageSender>();
            services.AddTransient<ISettingsService, SettingsService>();
            services.AddTransient<IDummyTwoFactorAuthService, DummyTwoFactorAuthService>();
        }
    }
}
