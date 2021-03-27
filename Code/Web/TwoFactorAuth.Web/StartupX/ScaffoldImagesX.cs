namespace TwoFactorAuth.Web.StartupX
{
    using Microsoft.AspNetCore.Builder;
    using Microsoft.Extensions.DependencyInjection;
    using TwoFactorAuth.Services.Auth.Contracts;

    static internal class ScaffoldImagesX
    {
        static internal IApplicationBuilder ScaffoldImages(this IApplicationBuilder app)
        {
            using var serviceScope = app
                .ApplicationServices
                .CreateScope();

            var passwordHintImageService = serviceScope
                .ServiceProvider
                .GetRequiredService<IPasswordHintImageService>();

            passwordHintImageService
                .SpawnSecondStagePasswordHintImageAsync()
                .GetAwaiter()
                .GetResult();

            return app;
        }
    }
}
