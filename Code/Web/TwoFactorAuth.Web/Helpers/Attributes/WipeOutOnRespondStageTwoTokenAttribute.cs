namespace TwoFactorAuth.Web.Helpers.Attributes
{
    using Microsoft.AspNetCore.Mvc.Filters;

    using TwoFactorAuth.Common;

    public class WipeOutOnRespondStageTwoTokenAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuted(ActionExecutedContext context)
        {
            try
            {
                context
                    .HttpContext
                    .Response
                    .Cookies
                    .Delete(key: GlobalConstants.DummyAuthSpecs.CookieSpecs.EnableAccessToSecondStage);
            }
            finally
            {
                base.OnActionExecuted(context);
            }
        }
    }
}