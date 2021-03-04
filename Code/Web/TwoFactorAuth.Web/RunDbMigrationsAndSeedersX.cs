namespace TwoFactorAuth.Web
{
    using Microsoft.AspNetCore.Builder;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.DependencyInjection;

    using TwoFactorAuth.Data;
    using TwoFactorAuth.Data.Seeding;

    static public class RunDbMigrationsAndSeedersX
    {
        static public IApplicationBuilder RunDbMigrationsAndSeeders(this IApplicationBuilder app)
        {
            using var serviceScope = app
                .ApplicationServices
                .CreateScope();
            
            var dbContext = serviceScope
                .ServiceProvider
                .GetRequiredService<ApplicationDbContext>();

            dbContext
                .Database
                .Migrate(); //order

            new ApplicationDbContextSeeder() //order
                .SeedAsync(dbContext, serviceScope.ServiceProvider)
                .GetAwaiter()
                .GetResult();

            return app;
        }
    }
}