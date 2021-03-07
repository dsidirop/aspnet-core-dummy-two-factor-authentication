namespace TwoFactorAuth.Web.StartupX
{
    using System.Reflection;

    using Microsoft.AspNetCore.Builder;

    using TwoFactorAuth.Services.Mapping;
    using TwoFactorAuth.Web.ViewModels;

    static internal class RegisterMappingsX
    {
        static internal void RegisterMappings(this IApplicationBuilder app)
        {
            AutoMapperConfig.RegisterMappings(typeof(ErrorViewModel).GetTypeInfo().Assembly);
        }
    }
}