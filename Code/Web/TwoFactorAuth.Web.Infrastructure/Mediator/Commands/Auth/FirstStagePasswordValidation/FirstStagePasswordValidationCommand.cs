namespace TwoFactorAuth.Web.Infrastructure.Mediator.Commands.Auth.FirstStagePasswordValidation
{
    using MediatR;

    public class FirstStagePasswordValidationCommand : IRequest<FirstStagePasswordValidationVerdict>
    {
        public FirstStagePasswordValidationCommand(string password)
        {
            Password = password;
        }

        public string Password { get; }
    }
}
