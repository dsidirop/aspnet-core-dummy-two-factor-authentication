namespace TwoFactorAuth.Web.Infrastructure.Mediator.Commands.Auth.FirstStagePasswordValidation
{
    public class FirstStagePasswordValidationVerdict
    {
        public FirstStagePasswordValidationVerdict(bool success)
        {
            Success = success;
        }

        public bool Success { get; }
    }
}
