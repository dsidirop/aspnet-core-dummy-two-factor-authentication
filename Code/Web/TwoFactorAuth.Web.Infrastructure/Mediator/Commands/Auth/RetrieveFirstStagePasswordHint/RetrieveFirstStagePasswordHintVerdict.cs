namespace TwoFactorAuth.Web.Infrastructure.Mediator.Commands.Auth.RetrieveFirstStagePasswordHint
{
    public class RetrieveFirstStagePasswordHintVerdict
    {
        public string EncodedPasswordHint { get; }
    
        public RetrieveFirstStagePasswordHintVerdict(string encodedPasswordHint)
        {
            EncodedPasswordHint = encodedPasswordHint;
        }
    }
}