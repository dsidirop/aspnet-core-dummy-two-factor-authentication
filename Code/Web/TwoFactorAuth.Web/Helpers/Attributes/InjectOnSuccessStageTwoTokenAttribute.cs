namespace TwoFactorAuth.Web.Helpers.Attributes
{
    using System;
    using System.Net;

    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc.Filters;

    using TwoFactorAuth.Common;

    public class InjectOnSuccessStageTwoTokenAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuted(ActionExecutedContext context)
        {
            try
            {
                var response = context.HttpContext.Response;
                if (response.StatusCode != (int) HttpStatusCode.OK && response.StatusCode != (int) HttpStatusCode.Redirect)
                    return;

                response.Cookies.Append(
                    key: GlobalConstants.DummyAuthUserSpecs.CookieSpecs.EnableAccessToSecondStage,
                    value: "some_value", //todo
                    new CookieOptions
                    {
                        Secure = true, //0 vital
                        MaxAge = TimeSpan.FromHours(2), //0
                        Expires = DateTimeOffset.Now.AddHours(2), //0 backwards compatibility ie8
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