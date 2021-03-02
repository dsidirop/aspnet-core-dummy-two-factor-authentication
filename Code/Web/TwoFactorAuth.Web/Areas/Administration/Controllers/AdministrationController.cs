namespace TwoFactorAuth.Web.Areas.Administration.Controllers
{
    using Common;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    using TwoFactorAuth.Web.Controllers;

    [Area("Administration")]
    [Authorize(Roles = GlobalConstants.AdministratorRoleName)]
    public class AdministrationController : BaseController
    {
    }
}
