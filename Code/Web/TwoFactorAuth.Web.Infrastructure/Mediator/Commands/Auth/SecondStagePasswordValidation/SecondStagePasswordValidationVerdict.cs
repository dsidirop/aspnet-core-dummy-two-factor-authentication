namespace TwoFactorAuth.Web.Infrastructure.Mediator.Commands.Auth.SecondStagePasswordValidation
{
    public class SecondStagePasswordValidationVerdict
    {
        public SecondStagePasswordValidationVerdict(bool success)
        {
            Success = success;
        }

        public bool Success { get; }
    }
}
