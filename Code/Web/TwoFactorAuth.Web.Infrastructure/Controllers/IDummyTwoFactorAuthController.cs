namespace TwoFactorAuth.Web.Infrastructure.Controllers
{
    public interface IDummyTwoFactorAuthController
    {
        public string ControllerName { get; }
        public string LoginStep1Action { get; }
    }
}
