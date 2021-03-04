namespace TwoFactorAuth.Web.Infrastructure.Attributes
{
    using System.Net;

    using Microsoft.AspNetCore.Mvc.Filters;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Options;

    using TwoFactorAuth.Common.Configuration;

    public class WipeOutOnSuccessStageTwoTokenAttribute : ActionFilterAttribute
    {
        private IOptionsMonitor<AppDummyAuthSpecs> _dummyAuthSpecsOptionsMonitor;

        public override void OnActionExecuted(ActionExecutedContext context)
        {
            _dummyAuthSpecsOptionsMonitor = context.HttpContext.RequestServices.GetService<IOptionsMonitor<AppDummyAuthSpecs>>();

            try
            {
                var response = context.HttpContext.Response;
                if (response.StatusCode != (int) HttpStatusCode.OK && response.StatusCode != (int) HttpStatusCode.Redirect)
                    return;

                response.Cookies.Delete(
                    _dummyAuthSpecsOptionsMonitor
                        .CurrentValue
                        .CookiesSettings
                        .CookieNameForEnableAccessToSecondStage
                );
            }
            finally
            {
                base.OnActionExecuted(context);
            }
        }
    }
}