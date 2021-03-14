namespace TwoFactorAuth.Web.Infrastructure.Mediator.Commands.Auth.FirstStagePasswordValidation
{
    using MediatR;

    public class FirstStagePasswordValidationCommand : IRequest<FirstStagePasswordValidationVerdict>
    {
        public string Password { get; }

        public FirstStagePasswordValidationCommand(string password)
        {
            Password = password;
        }
    }
}