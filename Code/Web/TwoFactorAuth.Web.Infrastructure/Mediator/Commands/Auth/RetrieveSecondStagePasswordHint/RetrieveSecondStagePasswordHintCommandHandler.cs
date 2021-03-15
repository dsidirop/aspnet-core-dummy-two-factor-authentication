// ReSharper disable UnusedMember.Global
namespace TwoFactorAuth.Web.Infrastructure.Mediator.Commands.Auth.RetrieveSecondStagePasswordHint
{
    using System.Threading;
    using System.Threading.Tasks;

    using MediatR;

    using TwoFactorAuth.Common.Contracts;
    using TwoFactorAuth.Services.Auth.Contracts;

    public class RetrieveSecondStagePasswordHintCommandHandler : IRequestHandler<RetrieveSecondStagePasswordHintCommand, RetrieveSecondStagePasswordHintVerdict>
    {
        private readonly IPasswordHintImageService _passwordHintImageService;

        public RetrieveSecondStagePasswordHintCommandHandler(IPasswordHintImageService passwordHintImageService)
        {
            _passwordHintImageService = passwordHintImageService;
        }

        public async Task<RetrieveSecondStagePasswordHintVerdict> Handle(RetrieveSecondStagePasswordHintCommand request, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            var path = _passwordHintImageService.GetLoginSecondStepImageHintFilePath();

            return await Task.FromResult(new RetrieveSecondStagePasswordHintVerdict(
                imagePasswordHintFilepath: path,
                imagePasswordHintMimeType: GlobalConstants.LoginSecondStepEventualImagePasswordHintMimeType
            ));
        }
    }
}