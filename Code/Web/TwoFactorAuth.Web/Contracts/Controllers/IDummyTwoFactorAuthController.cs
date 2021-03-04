namespace TwoFactorAuth.Web.Contracts.Controllers
{
    public interface IDummyTwoFactorAuthController
    {
        public string ControllerName { get; }
        public string LoginStep1Action { get; }
    }
}