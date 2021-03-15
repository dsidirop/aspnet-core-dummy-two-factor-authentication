namespace TwoFactorAuth.Web.Infrastructure.Mediator.Commands.Auth.RetrieveSecondStagePasswordHint
{
    public class RetrieveSecondStagePasswordHintVerdict
    {
        public string ImagePasswordHintFilepath { get; set; }
        public string ImagePasswordHintMimeType { get; set; }

        public RetrieveSecondStagePasswordHintVerdict(string imagePasswordHintFilepath, string imagePasswordHintMimeType)
        {
            ImagePasswordHintFilepath = imagePasswordHintFilepath;
            ImagePasswordHintMimeType = imagePasswordHintMimeType;
        }
    }
}