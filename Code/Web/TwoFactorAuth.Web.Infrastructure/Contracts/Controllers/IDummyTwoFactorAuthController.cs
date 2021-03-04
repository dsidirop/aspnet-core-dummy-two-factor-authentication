namespace TwoFactorAuth.Web.Infrastructure.Contracts.Controllers
{
    public interface IDummyTwoFactorAuthController
    {
        public string ControllerName { get; }
        public string LoginStep1Action { get; }
    }
}