// ReSharper disable UnusedMember.Global

namespace TwoFactorAuth.Web.Infrastructure.Mediator.Commands.Auth.FirstStagePasswordValidation
{
    using System.Threading;
    using System.Threading.Tasks;

    using MediatR;

    using TwoFactorAuth.Services.Auth.Contracts;

    public class FirstStagePasswordValidationCommandHandler : IRequestHandler<FirstStagePasswordValidationCommand, FirstStagePasswordValidationVerdict>
    {
        private readonly IDummyTwoFactorAuthService _authService;

        public FirstStagePasswordValidationCommandHandler(IDummyTwoFactorAuthService authService)
        {
            _authService = authService;
        }

        public async Task<FirstStagePasswordValidationVerdict> Handle(FirstStagePasswordValidationCommand request, CancellationToken cancellationToken)
        {
            var success = await _authService.FirstStageSignInAsync(request.Password);

            return new FirstStagePasswordValidationVerdict(success);
        }
    }
}