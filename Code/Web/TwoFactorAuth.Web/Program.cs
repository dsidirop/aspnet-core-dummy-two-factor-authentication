// ReSharper disable MemberCanBePrivate.Global

namespace TwoFactorAuth.Web
{
    using System;
    using Autofac.Extensions.DependencyInjection;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.Hosting;

    static public class Program
    {
        static public void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        static public IHostBuilder CreateHostBuilder(string[] args)
        {
            return Host
                .CreateDefaultBuilder(args)
                .UseServiceProviderFactory(new AutofacServiceProviderFactory())
                .ConfigureAppConfiguration((hostingContext, config) =>
                {
                    var environmentName = hostingContext.HostingEnvironment.EnvironmentName; //0                       development production etc
                    var particularLaunchMethod = Environment.GetEnvironmentVariable("ASPNETCORE_LAUNCH_METHOD"); //0   docker iisexpress etc

                    config
                        .SetBasePath(basePath: hostingContext.HostingEnvironment.ContentRootPath)
                        .AddJsonFile(
                            path: $"appsettings.{environmentName}.{particularLaunchMethod}.json", //mainly for appsettings.development.docker.json
                            optional: true,
                            reloadOnChange: false
                        );
                })
                .ConfigureWebHostDefaults(webBuilder => { webBuilder.UseStartup<Startup>(); });

            //0 we set the variables aspnetcore_environment and aspnetcore_launch_method by opening the dialog of debug-properties
            //  or by directly editing the file launchSettings.json
        }
    }
}
