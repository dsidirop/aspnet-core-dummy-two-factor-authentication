namespace TwoFactorAuth.Web.StartupX
{
    using System.Globalization;

    using Microsoft.AspNetCore.Builder;

    static internal class SetDefaultThreadCultureX
    {
        static internal void SetDefaultThreadCulture(this IApplicationBuilder app)
        {
            var cultureInfo = CultureInfo.InvariantCulture; //best practice

            CultureInfo.DefaultThreadCurrentCulture = cultureInfo;
            CultureInfo.DefaultThreadCurrentUICulture = cultureInfo;
        }
    }
}
