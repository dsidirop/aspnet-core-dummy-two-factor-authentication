namespace TwoFactorAuth.Web.Infrastructure.Attributes
{
    using System;
    using System.Net;

    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc.Filters;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Options;

    using TwoFactorAuth.Common.Contracts.Configuration;
    using TwoFactorAuth.Services.Contracts;
    using TwoFactorAuth.Web.Infrastructure.Attributes.CustomCookies;

    public class InjectOnSuccessStageTwoTokenAttribute : ActionFilterAttribute
    {
        private IAppCryptoService _cryptoService;
        private IOptionsMonitor<AppDummyAuthSpecs> _dummyAuthSpecsOptionsMonitor;

        public override void OnActionExecuted(ActionExecutedContext context)
        {
            _cryptoService = context.HttpContext.RequestServices.GetService<IAppCryptoService>();
            _dummyAuthSpecsOptionsMonitor = context.HttpContext.RequestServices.GetService<IOptionsMonitor<AppDummyAuthSpecs>>();

            try
            {
                var response = context.HttpContext.Response;
                if (response.StatusCode != (int) HttpStatusCode.OK && response.StatusCode != (int) HttpStatusCode.Redirect)
                    return;

                var cookieLifespan = _dummyAuthSpecsOptionsMonitor
                    .CurrentValue
                    .CookiesSettings
                    .SecondStageEnablingCookieLifespanInMins;

                var nowUtc = DateTimeOffset.UtcNow;
                var secondStageEnablingCookieValueJson = System.Text.Json.JsonSerializer.Serialize(
                    new DummyAuthSecondStageEnablingCookieSpecs
                    {
                        ExpiresAt = nowUtc.AddMinutes(cookieLifespan),
                    }
                );

                var encryptedValue = _cryptoService.EncryptToBase64String(secondStageEnablingCookieValueJson);
                
                response.Cookies.Append(
                    key: _dummyAuthSpecsOptionsMonitor.CurrentValue.CookiesSettings.CookieNameForEnableAccessToSecondStage,
                    value: encryptedValue,
                    new CookieOptions
                    {
                        Secure = true, //0 vital
                        MaxAge = TimeSpan.FromMinutes(cookieLifespan),
                        Expires = nowUtc.AddHours(cookieLifespan), //backwards compatibility ie8
                        IsEssential = false,
                    }
                );
            }
            finally
            {
                base.OnActionExecuted(context);
            }

            //0 using site=none mandates setting secure=true otherwise the cookie will get rejected by chrome
        }
    }
}