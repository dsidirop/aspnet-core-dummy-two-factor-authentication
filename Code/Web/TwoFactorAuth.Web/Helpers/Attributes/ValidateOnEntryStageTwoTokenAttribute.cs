using Microsoft.AspNetCore.Mvc.Filters;

namespace TwoFactorAuth.Web.Helpers.Attributes
{
    public class ValidateOnEntryStageTwoTokenAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            //todo

            base.OnActionExecuting(context);
        }
    }
}