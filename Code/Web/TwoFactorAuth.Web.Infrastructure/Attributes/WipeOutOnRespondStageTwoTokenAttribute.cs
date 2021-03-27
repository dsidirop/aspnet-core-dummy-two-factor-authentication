namespace TwoFactorAuth.Web.Infrastructure.Attributes
{
    using Microsoft.AspNetCore.Mvc.Filters;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Options;
    using TwoFactorAuth.Common.Contracts.Configuration;

    public class WipeOutOnRespondStageTwoTokenAttribute : ActionFilterAttribute
    {
        private IOptionsMonitor<AppDummyAuthSpecs> _dummyAuthSpecsOptionsMonitor;

        public override void OnActionExecuted(ActionExecutedContext context)
        {
            _dummyAuthSpecsOptionsMonitor = context.HttpContext.RequestServices.GetService<IOptionsMonitor<AppDummyAuthSpecs>>();

            try
            {
                context
                    .HttpContext
                    .Response
                    .Cookies
                    .Delete(_dummyAuthSpecsOptionsMonitor.CurrentValue.CookiesSettings.CookieNameForEnableAccessToSecondStage);
            }
            finally
            {
                base.OnActionExecuted(context);
            }
        }
    }
}
