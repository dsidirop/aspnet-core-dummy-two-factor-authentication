// ReSharper disable UnusedMember.Global
namespace TwoFactorAuth.Web.Infrastructure.Mediator.Commands.Auth.RetrieveFirstStagePasswordHint
{
    using System.Threading;
    using System.Threading.Tasks;

    using MediatR;

    using Microsoft.Extensions.Options;

    using TwoFactorAuth.Common;
    using TwoFactorAuth.Common.Contracts.Configuration;

    public class RetrieveFirstStagePasswordHintCommandHandler : IRequestHandler<RetrieveFirstStagePasswordHintCommand, RetrieveFirstStagePasswordHintVerdict>
    {
        private readonly AppDummyAuthSpecs _authSpecs;

        public RetrieveFirstStagePasswordHintCommandHandler(IOptionsMonitor<AppDummyAuthSpecs> authSpecsOptionsMonitor)
        {
            _authSpecs = authSpecsOptionsMonitor.CurrentValue;
        }

        public async Task<RetrieveFirstStagePasswordHintVerdict> Handle(RetrieveFirstStagePasswordHintCommand request, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            var encodedPassword = _authSpecs.DummyUsers.First.Password.Asciify();

            return await Task.FromResult(new RetrieveFirstStagePasswordHintVerdict(encodedPassword));
        }
    }
}