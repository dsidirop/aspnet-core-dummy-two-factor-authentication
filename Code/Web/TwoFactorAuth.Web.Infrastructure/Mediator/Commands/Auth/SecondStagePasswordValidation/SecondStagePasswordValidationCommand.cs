namespace TwoFactorAuth.Web.Infrastructure.Mediator.Commands.Auth.SecondStagePasswordValidation
{
    using MediatR;

    public class SecondStagePasswordValidationCommand : IRequest<SecondStagePasswordValidationVerdict>
    {
        public string Password { get; }

        public SecondStagePasswordValidationCommand(string password)
        {
            Password = password;
        }
    }
}