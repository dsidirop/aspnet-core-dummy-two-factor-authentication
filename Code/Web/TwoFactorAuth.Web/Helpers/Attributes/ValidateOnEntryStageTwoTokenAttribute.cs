using TwoFactorAuth.Web.Controllers;

namespace TwoFactorAuth.Web.Helpers.Attributes
{
    using System;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Filters;

    using TwoFactorAuth.Common;
    using TwoFactorAuth.Web.Areas.Identity.Controllers;

    public class ValidateOnEntryStageTwoTokenAttribute : ActionFilterAttribute
    {
        public ValidateOnEntryStageTwoTokenAttribute()
        {
            Order = 2000; //0 best practice

            //0 the default order for this attribute is 2000 because it must run after any filter which does authentication 
            //  or login or baseline antiforgery checks in order to allow them to behave as expected   ie Unauthenticated or
            //  Redirect instead of 400
        }

        public override async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var controller = context.Controller as IDummyTwoFactorAuthController;
            if (controller == null)
                throw new ArgumentException("This filter is specific to [I]DummyTwoFactorAuthController - no other controller should be using it", nameof(context));

            try
            {
                var cookieFound = context
                    .HttpContext
                    .Request
                    .Cookies
                    .TryGetValue(
                        key: GlobalConstants.DummyAuthUserSpecs.CookieSpecs.EnableAccessToSecondStage,
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
                    return;
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

        static private bool ValidateCookieValue(string cookie)
        {
            //todo

            return true;
        }
    }
}