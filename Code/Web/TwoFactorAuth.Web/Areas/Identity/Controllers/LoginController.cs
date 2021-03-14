﻿namespace TwoFactorAuth.Web.Areas.Identity.Controllers
{
    using System.Net;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Options;

    using TwoFactorAuth.Common;
    using TwoFactorAuth.Common.Contracts;
    using TwoFactorAuth.Common.Contracts.Configuration;
    using TwoFactorAuth.Services.Auth.Contracts;
    using TwoFactorAuth.Web.Controllers;
    using TwoFactorAuth.Web.Infrastructure.Attributes;
    using TwoFactorAuth.Web.Infrastructure.Controllers;
    using TwoFactorAuth.Web.ViewModels.Login;

    [AllowAnonymous]
    [Area("Identity")]
    public class LoginController : PlatformBaseController, IDummyTwoFactorAuthController
    {
        public string ControllerName { get; } = nameof(LoginController).Replace("Controller", "");
        public string LoginStep1Action { get; } = nameof(Index);

        private readonly AppDummyAuthSpecs _authSpecs;
        private readonly IPasswordHintImageService _passwordHintImageService;
        private readonly IDummyTwoFactorAuthService _authService;

        public LoginController(
            IPasswordHintImageService passwordHintImageService,
            IDummyTwoFactorAuthService authService,
            IOptionsMonitor<AppDummyAuthSpecs> authSpecsOptionsMonitor

        )
        {
            _authSpecs = authSpecsOptionsMonitor.CurrentValue;
            _authService = authService;
            _passwordHintImageService = passwordHintImageService;
        }

        #region step1

        [HttpGet]
        [WipeOutOnRespondStageTwoToken] //0
        public IActionResult Index() //login step1
        {
            return View("Index", new LoginStep1ViewModel
            {
                HiddenEncodedPassword = _authSpecs.DummyUsers.First.Password.Asciify(),
            });

            //0 if the user for whatever reason revisits the first stage deliberately then we wipe out any preexisting stage two token
            //  he might have
        }

        [HttpPost]
        [InjectOnSuccessStageTwoToken] //0
        public async Task<IActionResult> Index(string password)
        {
            var success = await _authService.FirstStageSignInAsync(password);
            if (!success)
            {
                ModelState.AddModelError("Password", "Wrong Password!");

                Response.StatusCode = (int)HttpStatusCode.Forbidden; //vital
                return View("Index");
            }

            return RedirectToAction(nameof(LoginSecondStep));

            //0 if the action is green then the attribute injects the client with an antiforgery token-cookie to enable him to access SecondStep
        }

        #endregion



        #region SecondStep

        [HttpGet]
        [ValidateOnEntryStageTwoToken] //vital
        public IActionResult LoginSecondStep()
        {
            return View(new LoginSecondStepViewModel
            {
                PasswordHintImagePath = Url.Action(GlobalConstants.LoginSecondStepImageHintActionName),
            });
        }

        [HttpGet]
        [ValidateOnEntryStageTwoToken]
        [ActionName(GlobalConstants.LoginSecondStepImageHintActionName)]
        public IActionResult PasswordHintImage()
        {
            var imagePasswordHintFilepath = _passwordHintImageService.GetLoginSecondStepImageHintFilePath(); //0

            return PhysicalFile(imagePasswordHintFilepath, GlobalConstants.LoginSecondStepEventualImagePasswordHintMimeType);

            //0 C:\VS\aspnet-core-dummy-two-factor-authentication\Code\Web\TwoFactorAuth.Web\content\image.jpg
        }

        [HttpPost]
        [ValidateOnEntryStageTwoToken]
        [WipeOutOnSuccessStageTwoToken] //0 vital
        public async Task<IActionResult> LoginSecondStep(string password)
        {
            var success = await _authService.SecondStageSignInAsync(HttpContext, password, isPersistent: true);
            if (!success)
            {
                ModelState.AddModelError("Password", "Wrong Password!");

                Response.StatusCode = (int)HttpStatusCode.Forbidden; //vital
                return View();
            }

            return RedirectToAction(nameof(HomeController.Welldone), "Home", new { area = "" });

            //0 if the action is green then the stage two cookie needs to be eradicated so as to prevent the user from skipping the
            //  first stage next time he logs in
        }

        #endregion
    }
}
