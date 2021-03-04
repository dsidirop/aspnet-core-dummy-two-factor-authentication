namespace TwoFactorAuth.Web.Helpers.Attributes
{
    using System.Net;

    using Microsoft.AspNetCore.Mvc.Filters;

    using TwoFactorAuth.Common;

    public class WipeOutOnSuccessStageTwoTokenAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuted(ActionExecutedContext context)
        {
            try
            {
                var response = context.HttpContext.Response;
                if (response.StatusCode != (int) HttpStatusCode.OK && response.StatusCode != (int) HttpStatusCode.Redirect)
                    return;

                response.Cookies.Delete(GlobalConstants.DummyAuthSpecs.CookieSpecs.EnableAccessToSecondStage);
            }
            finally
            {
                base.OnActionExecuted(context);
            }
        }
    }
}