namespace TwoFactorAuth.Web.Areas.Administration.Controllers
{
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    using TwoFactorAuth.Common.Contracts;
    using TwoFactorAuth.Web.Infrastructure.Controllers;

    [Area("Administration")]
    [Authorize(Roles = GlobalConstants.AdministratorRoleName)]
    public class AdministrationController : PlatformBaseController
    {
    }
}
