namespace TwoFactorAuth.Web.Controllers
{
    using System.Diagnostics;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using TwoFactorAuth.Common.Contracts;
    using TwoFactorAuth.Web.Infrastructure.Attributes;
    using TwoFactorAuth.Web.Infrastructure.Controllers;
    using TwoFactorAuth.Web.ViewModels;
    using TwoFactorAuth.Web.ViewModels.Home;

    public class HomeController : PlatformBaseController
    {
        public IActionResult Index()
        {
            return View();
        }

        [Authorize]
        public IActionResult Welldone()
        {
            return View(new WelldoneViewModel
            {
                GreenTickImagePath = GlobalConstants.GreenTickImagePathRelativeToRoot,
            });
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [DontCache]
        public IActionResult Error()
        {
            return View(new ErrorViewModel
            {
                RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier,
            });
        }
    }
}
