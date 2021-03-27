namespace TwoFactorAuth.Web.Infrastructure.Mediator.Commands.Auth.RetrieveFirstStagePasswordHint
{
    public class RetrieveFirstStagePasswordHintVerdict
    {
        public RetrieveFirstStagePasswordHintVerdict(string encodedPasswordHint)
        {
            EncodedPasswordHint = encodedPasswordHint;
        }

        public string EncodedPasswordHint { get; }
    }
}
