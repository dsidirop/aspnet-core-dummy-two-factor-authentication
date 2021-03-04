namespace TwoFactorAuth.Web
{
    using System.Globalization;

    using Microsoft.AspNetCore.Builder;

    static public class SetDefaultThreadCultureX
    {
        static public void SetDefaultThreadCulture(this IApplicationBuilder app)
        {
            var cultureInfo = CultureInfo.InvariantCulture; //best practice

            CultureInfo.DefaultThreadCurrentCulture = cultureInfo;
            CultureInfo.DefaultThreadCurrentUICulture = cultureInfo;
        }
    }
}
