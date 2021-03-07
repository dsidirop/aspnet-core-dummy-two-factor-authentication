namespace TwoFactorAuth.Web
{
    using Autofac.Extensions.DependencyInjection;

    using Microsoft.AspNetCore.Hosting;
    using Microsoft.Extensions.Hosting;

    static public class Program
    {
        static public void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        static public IHostBuilder CreateHostBuilder(string[] args) => Host
            .CreateDefaultBuilder(args)
            .UseServiceProviderFactory(new AutofacServiceProviderFactory())
            .ConfigureWebHostDefaults(webBuilder =>
            {
                webBuilder.UseStartup<Startup>();
            });
    }
}
