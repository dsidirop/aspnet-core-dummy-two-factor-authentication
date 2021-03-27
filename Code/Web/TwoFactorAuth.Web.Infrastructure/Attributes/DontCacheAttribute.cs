namespace TwoFactorAuth.Web.Infrastructure.Attributes
{
    using Microsoft.AspNetCore.Mvc;

    public sealed class DontCacheAttribute : ResponseCacheAttribute
    {
        public DontCacheAttribute()
        {
            NoStore = true;
            Duration = 0;
            Location = ResponseCacheLocation.None;
        }
    }
}
