namespace TwoFactorAuth.Web
{
    using System.Reflection;

    using Microsoft.AspNetCore.Builder;

    using TwoFactorAuth.Services.Mapping;
    using TwoFactorAuth.Web.ViewModels;

    static public class RegisterMappingsX
    {
        static public void RegisterMappings(this IApplicationBuilder app)
        {
            AutoMapperConfig.RegisterMappings(typeof(ErrorViewModel).GetTypeInfo().Assembly);
        }
    }
}