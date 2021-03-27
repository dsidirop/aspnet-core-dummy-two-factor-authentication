namespace TwoFactorAuth.Web.Infrastructure.Mediator.Commands.Auth.SecondStagePasswordValidation
{
    using MediatR;

    public class SecondStagePasswordValidationCommand : IRequest<SecondStagePasswordValidationVerdict>
    {
        public SecondStagePasswordValidationCommand(string password)
        {
            Password = password;
        }

        public string Password { get; }
    }
}
