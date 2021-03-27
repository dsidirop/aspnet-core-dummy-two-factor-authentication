// ReSharper disable UnusedMember.Global

namespace TwoFactorAuth.Web.Infrastructure.Mediator.Commands.Auth.SecondStagePasswordValidation
{
    using System.Threading;
    using System.Threading.Tasks;
    using MediatR;
    using TwoFactorAuth.Services.Auth.Contracts;

    public class SecondStagePasswordValidationCommandHandler : IRequestHandler<SecondStagePasswordValidationCommand, SecondStagePasswordValidationVerdict>
    {
        private readonly IDummyTwoFactorAuthService _authService;

        public SecondStagePasswordValidationCommandHandler(IDummyTwoFactorAuthService authService)
        {
            _authService = authService;
        }

        public async Task<SecondStagePasswordValidationVerdict> Handle(SecondStagePasswordValidationCommand request, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            var success = await _authService.SecondStageSignInAsync(request.Password, true);

            return new SecondStagePasswordValidationVerdict(success);
        }
    }
}
