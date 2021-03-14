namespace TwoFactorAuth.Web.Infrastructure.Mediator.Commands.Auth.FirstStagePasswordValidation
{
    public class FirstStagePasswordValidationVerdict
    {
        public bool Success { get; }
    
        public FirstStagePasswordValidationVerdict(bool success)
        {
            Success = success;
        }
    }
}