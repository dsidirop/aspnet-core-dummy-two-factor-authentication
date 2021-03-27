namespace TwoFactorAuth.Web.Areas.Identity.Controllers
{
    using System.Net;
    using System.Threading;
    using System.Threading.Tasks;
    using MediatR;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using TwoFactorAuth.Common.Contracts;
    using TwoFactorAuth.Web.Controllers;
    using TwoFactorAuth.Web.Infrastructure.Attributes;
    using TwoFactorAuth.Web.Infrastructure.Controllers;
    using TwoFactorAuth.Web.Infrastructure.Mediator.Commands.Auth.FirstStagePasswordValidation;
    using TwoFactorAuth.Web.Infrastructure.Mediator.Commands.Auth.RetrieveFirstStagePasswordHint;
    using TwoFactorAuth.Web.Infrastructure.Mediator.Commands.Auth.RetrieveSecondStagePasswordHint;
    using TwoFactorAuth.Web.Infrastructure.Mediator.Commands.Auth.SecondStagePasswordValidation;
    using TwoFactorAuth.Web.ViewModels.Login;

    [AllowAnonymous]
    [Area("Identity")]
    public class LoginController : PlatformBaseController, IDummyTwoFactorAuthController
    {
        private readonly IMediator _mediator;

        public LoginController(IMediator mediator)
        {
            _mediator = mediator;
        }

        public string ControllerName { get; } = nameof(LoginController).Replace("Controller", "");
        public string LoginStep1Action { get; } = nameof(Index);

        #region step1

        [HttpGet]
        [WipeOutOnRespondStageTwoToken] //0
        public async Task<IActionResult> Index() //login step1
        {
            var result = await _mediator.Send(new RetrieveFirstStagePasswordHintCommand());

            return View(new LoginStep1ViewModel
            {
                HiddenEncodedPassword = result.EncodedPasswordHint,
            });

            //0 if the user for whatever reason revisits the first stage deliberately then we wipe out any preexisting stage two token
            //  he might have
        }

        [HttpPost]
        [InjectOnSuccessStageTwoToken] //0
        public async Task<IActionResult> Index(string password)
        {
            var verdict = await _mediator.Send(new FirstStagePasswordValidationCommand(password));
            if (!verdict.Success)
            {
                ModelState.AddModelError("Password", "Wrong Password!");

                Response.StatusCode = (int) HttpStatusCode.Forbidden; //vital
                return await Index(); //vital
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
        [ActionName(GlobalConstants.LoginSecondStepImageHintActionName)] // password-hint.jpg
        public async Task<IActionResult> PasswordHintImage(CancellationToken cancellationToken)
        {
            var verdict = await _mediator.Send(new RetrieveSecondStagePasswordHintCommand(), cancellationToken);

            return PhysicalFile(verdict.ImagePasswordHintFilepath, verdict.ImagePasswordHintMimeType); //0

            //0 C:\VS\aspnet-core-dummy-two-factor-authentication\Code\Web\TwoFactorAuth.Web\content\image.jpg
        }

        [HttpPost]
        [ValidateOnEntryStageTwoToken]
        [WipeOutOnSuccessStageTwoToken] //0 vital
        public async Task<IActionResult> LoginSecondStep(string password)
        {
            var verdict = await _mediator.Send(new SecondStagePasswordValidationCommand(password));
            if (!verdict.Success)
            {
                ModelState.AddModelError("Password", "Wrong Password!");

                Response.StatusCode = (int) HttpStatusCode.Forbidden; //  vital
                return LoginSecondStep(); //                             vital
            }

            return RedirectToAction(nameof(HomeController.Welldone), "Home", new {area = ""});

            //0 if the action is green then the stage two cookie needs to be eradicated so as to prevent the user from skipping the
            //  first stage next time he logs in
        }

        #endregion
    }
}
