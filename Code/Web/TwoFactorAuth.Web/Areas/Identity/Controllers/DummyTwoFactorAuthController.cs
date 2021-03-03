namespace TwoFactorAuth.Web.Areas.Identity.Controllers
{
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    using TwoFactorAuth.Common;
    using TwoFactorAuth.Services.Data.DummyAuthService;
    using TwoFactorAuth.Web.Areas.Administration.Controllers;
    using TwoFactorAuth.Web.Controllers;
    using TwoFactorAuth.Web.ViewModels.DummyAuthUserLogin;

    [AllowAnonymous]
    [Area("Identity")]
    public class DummyTwoFactorAuthController : AdministrationController
    {
        private readonly IDummyTwoFactorAuthService _authService;

        public DummyTwoFactorAuthController(IDummyTwoFactorAuthService authService)
        {
            _authService = authService;
        }

        #region step1

        [HttpGet]
        public IActionResult Index() //login step1
        {
            return View("Index", new LoginStep1ViewModel
            {
                HiddenEncodedPassword = GlobalConstants.DummyAuthUserSpecs.Passwords.First.Asciify(),
            });
        }

        [HttpPost]
        public IActionResult Index(string password)
        {
            var success = _authService.FirstStageSignIn(password);
            if (!success)
            {
                ModelState.AddModelError("Password", "Wrong Password!");
                return View("Index");
            }

            return RedirectToAction(nameof(LoginStep2));
        }

        #endregion



        #region step2

        [HttpGet] //todo   introduce a special antiforgerytoken here to prevent direct access to stage2
        public IActionResult LoginStep2()
        {
            return View();
        }

        [HttpPost] //todo   introduce a special antiforgerytoken here to prevent direct access to stage2
        public async Task<IActionResult> LoginStep2(string password)
        {
            var success = await _authService.SecondStageSignInAsync(HttpContext, password, isPersistent: true);
            if (!success)
            {
                ModelState.AddModelError("", "Wrong Password!");
                return View();
            }

            return RedirectToAction(nameof(HomeController.Index), "Home");
        }

        #endregion
    }
}
