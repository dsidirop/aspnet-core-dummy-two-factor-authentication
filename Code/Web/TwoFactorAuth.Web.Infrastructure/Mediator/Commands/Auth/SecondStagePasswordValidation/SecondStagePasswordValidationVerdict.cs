namespace TwoFactorAuth.Web.Infrastructure.Mediator.Commands.Auth.SecondStagePasswordValidation
{
    public class SecondStagePasswordValidationVerdict
    {
        public bool Success { get; }
    
        public SecondStagePasswordValidationVerdict(bool success)
        {
            Success = success;
        }
    }
}