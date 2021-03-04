namespace TwoFactorAuth.Web.Infrastructure.Attributes
{
    using System.Linq;

    using Microsoft.AspNetCore.Mvc.Abstractions;
    using Microsoft.AspNetCore.Mvc.ActionConstraints;
    using Microsoft.AspNetCore.Routing;

    public class AjaxOnlyAttribute : ActionMethodSelectorAttribute
    {
        public override bool IsValidForRequest(RouteContext routeContext, ActionDescriptor actionDescriptor)
        {
            return routeContext
                       .HttpContext.Request
                       .Headers.TryGetValue("X-Requested-With", out var requestedWithHeader)
                   && requestedWithHeader.Contains("XMLHttpRequest");
        }
    }
}
