namespace TwoFactorAuth.Web.Controllers
{
    using System.Diagnostics;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using TwoFactorAuth.Web.Infrastructure.Attributes;
    using TwoFactorAuth.Web.Infrastructure.Controllers;
    using TwoFactorAuth.Web.ViewModels;

    public class HomeController : PlatformBaseController
    {
        public IActionResult Index()
        {
            return View();
        }

        [Authorize]
        public IActionResult Welldone()
        {
            return View();
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
