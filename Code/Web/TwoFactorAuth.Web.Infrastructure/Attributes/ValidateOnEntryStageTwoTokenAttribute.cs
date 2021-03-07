namespace TwoFactorAuth.Web.Infrastructure.Attributes
{
    using System;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Mvc.Filters;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Options;

    using TwoFactorAuth.Common.Configuration;
    using TwoFactorAuth.Services.Contracts;
    using TwoFactorAuth.Web.Infrastructure.Attributes.CustomCookies;
    using TwoFactorAuth.Web.Infrastructure.Contracts.Controllers;
    using TwoFactorAuth.Web.Infrastructure.Controllers;

    public class ValidateOnEntryStageTwoTokenAttribute : ActionFilterAttribute
    {
        private IAppCryptoService _cryptoService;
        private IOptionsMonitor<AppDummyAuthSpecs> _dummyAuthSpecsOptionsMonitor;

        public ValidateOnEntryStageTwoTokenAttribute()
        {
            Order = 2000; //0 best practice

            //0 the default order for this attribute is 2000 because it must run after any filter which does authentication 
            //  or login or baseline antiforgery checks in order to allow them to behave as expected   ie Unauthenticated or
            //  Redirect instead of 400
        }

        public override async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            _cryptoService = context.HttpContext.RequestServices.GetService<IAppCryptoService>();
            _dummyAuthSpecsOptionsMonitor = context.HttpContext.RequestServices.GetService<IOptionsMonitor<AppDummyAuthSpecs>>();

            var controller = context.Controller as IDummyTwoFactorAuthController;
            if (controller == null)
                throw new ArgumentException("This filter is specific to [I]LoginController - no other controller should be using it", nameof(context));

            try
            {
                var cookieFound = context
                    .HttpContext
                    .Request
                    .Cookies
                    .TryGetValue(
                        key: _dummyAuthSpecsOptionsMonitor.CurrentValue.CookiesSettings.CookieNameForEnableAccessToSecondStage,
                        value: out var cookie
                    );
                if (!cookieFound)
                {
                    SetRedirectionToLoginFirstStage(context, controller);
                    return;
                }

                var isValid = ValidateCookieValue(cookie);
                if (!isValid)
                {
                    SetRedirectionToLoginFirstStage(context, controller);
                }
            }
            finally
            {
                await base.OnActionExecutionAsync(context, next);
            }
        }

        static private void SetRedirectionToLoginFirstStage(ActionExecutingContext context, IDummyTwoFactorAuthController controller)
        {
            context.Result = (controller as PlatformBaseController).RedirectToAction(
                actionName: controller.LoginStep1Action, //first stage
                controllerName: controller.ControllerName
            );
        }

        private bool ValidateCookieValue(string cookie)
        {
            try
            {
                var decryptedValue = _cryptoService.DecryptFromBase64String(cookie);

                var dummyAuthSecondStageEnablingCookieSpecs = System
                    .Text
                    .Json
                    .JsonSerializer
                    .Deserialize<DummyAuthSecondStageEnablingCookieSpecs>(decryptedValue);

                return dummyAuthSecondStageEnablingCookieSpecs != null
                       && dummyAuthSecondStageEnablingCookieSpecs.ExpiresAt >= DateTime.UtcNow;
            }
            catch
            {
                return false;
            }
        }
    }
}