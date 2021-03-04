namespace TwoFactorAuth.Web.Helpers.Attributes
{
    using System;
    using System.Net;

    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc.Filters;
    using Microsoft.Extensions.DependencyInjection;

    using TwoFactorAuth.Common;
    using TwoFactorAuth.Services.Crypto;
    using TwoFactorAuth.Web.Helpers.Attributes.CustomCookies;

    public class InjectOnSuccessStageTwoTokenAttribute : ActionFilterAttribute
    {
        private IAppCryptoService _cryptoService;

        public override void OnActionExecuted(ActionExecutedContext context)
        {
            _cryptoService = context.HttpContext.RequestServices.GetService<IAppCryptoService>();

            try
            {
                var response = context.HttpContext.Response;
                if (response.StatusCode != (int) HttpStatusCode.OK && response.StatusCode != (int) HttpStatusCode.Redirect)
                    return;

                var secondStageEnablingCookieValueJson = System.Text.Json.JsonSerializer.Serialize(
                    new DummyAuthSecondStageEnablingCookieSpecs
                    {
                        ExpiresAt = DateTime.UtcNow.AddMinutes(GlobalConstants.DummyAuthSpecs.CookieSpecs.SecondStageEnablingCookieLifespanInMins),
                    }
                );

                var encryptedValue = _cryptoService.EncryptToBase64String(secondStageEnablingCookieValueJson);
                
                response.Cookies.Append(
                    key: GlobalConstants.DummyAuthSpecs.CookieSpecs.EnableAccessToSecondStage,
                    value: encryptedValue,
                    new CookieOptions
                    {
                        Secure = true, //0 vital
                        MaxAge = TimeSpan.FromMinutes(GlobalConstants.DummyAuthSpecs.CookieSpecs.SecondStageEnablingCookieLifespanInMins), //0
                        Expires = DateTimeOffset.Now.AddHours(GlobalConstants.DummyAuthSpecs.CookieSpecs.SecondStageEnablingCookieLifespanInMins), //0 backwards compatibility ie8
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