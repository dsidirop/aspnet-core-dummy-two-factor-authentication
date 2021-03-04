namespace TwoFactorAuth.Web.Areas.Identity.Controllers
{
    using System.Net;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    using TwoFactorAuth.Common;
    using TwoFactorAuth.Services.Data.DummyAuthService;
    using TwoFactorAuth.Web.Controllers;
    using TwoFactorAuth.Web.Helpers.Attributes;
    using TwoFactorAuth.Web.ViewModels.DummyAuthUserLogin;

    [AllowAnonymous]
    [Area("Identity")]
    public class DummyTwoFactorAuthController : PlatformBaseController, IDummyTwoFactorAuthController
    {
        public string ControllerName { get; } = nameof(DummyTwoFactorAuthController).Replace("Controller", "");
        public string LoginStep1Action { get; } = nameof(Index);

        private readonly IDummyTwoFactorAuthService _authService;

        public DummyTwoFactorAuthController(IDummyTwoFactorAuthService authService)
        {
            _authService = authService;
        }

        #region step1

        [HttpGet]
        [WipeOutOnRespondStageTwoToken] //0
        public IActionResult Index() //login step1
        {
            return View("Index", new LoginStep1ViewModel
            {
                HiddenEncodedPassword = GlobalConstants.DummyAuthUserSpecs.Passwords.First.Asciify(),
            });

            //0 if the user for whatever reason revisits the first stage deliberately then we wipe out any preexisting stage two token he might have
        }

        [HttpPost]
        [InjectOnSuccessStageTwoToken] //0
        public IActionResult Index(string password)
        {
            var success = _authService.FirstStageSignIn(password);
            if (!success)
            {
                ModelState.AddModelError("Password", "Wrong Password!");
                
                Response.StatusCode = (int) HttpStatusCode.BadRequest; //vital
                return View("Index");
            }
            
            return RedirectToAction(nameof(LoginStep2));

            //0 if the action is green then the attribute injects the client with an antiforgery token-cookie to enable him to access step2
        }

        #endregion



        #region step2

        [HttpGet]
        [ValidateOnEntryStageTwoToken]
        public IActionResult LoginStep2()
        {
            return View();
        }

        [HttpPost]
        [ValidateOnEntryStageTwoToken]
        [WipeOutOnSuccessStageTwoToken] //0
        public async Task<IActionResult> LoginStep2(string password)
        {
            var success = await _authService.SecondStageSignInAsync(HttpContext, password, isPersistent: true);
            if (!success)
            {
                ModelState.AddModelError("Password", "Wrong Password!");

                Response.StatusCode = (int) HttpStatusCode.BadRequest; //vital
                return View();
            }

            return RedirectToAction(nameof(HomeController.Welldone), "Home", new { area = "" });

            //0 if the action is green then the stage two cookie needs to be eradicated so as to prevent the user from skipping the
            //  first stage next time he logs in
        }

        #endregion
    }

    public interface IDummyTwoFactorAuthController
    {
        public string ControllerName { get; }
        public string LoginStep1Action { get; }
    }
}
