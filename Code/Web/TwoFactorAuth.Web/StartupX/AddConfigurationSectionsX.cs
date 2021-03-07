namespace TwoFactorAuth.Web.StartupX
{
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;

    using TwoFactorAuth.Common.Contracts.Configuration;

    static internal class AddConfigurationSectionsX
    {
        static public IServiceCollection AddConfigurationSections(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<AppCryptoConfig>(configuration.GetSection("Crypto"));
            services.Configure<AppDummyAuthSpecs>(configuration.GetSection("DummyAuthSpecs"));

            return services;
        }
    }
}