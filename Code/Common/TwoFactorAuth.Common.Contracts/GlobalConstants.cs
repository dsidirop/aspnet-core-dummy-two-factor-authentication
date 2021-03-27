namespace TwoFactorAuth.Common.Contracts
{
    static public class GlobalConstants
    {
        public const string SystemName = "TwoFactorAuth";
        public const string AdministratorRoleName = "Administrator";

        public const string LoginSecondStepImageHintActionName = "password-hint.jpg"; //for the controller

        public const string GreenTickImagePathRelativeToRoot = "/images/green-tick.gif";

        public const string LoginSecondStepBaseImagePasswordHintFilePath = "/wwwroot/images/blue-hint.orig.jpg";
        public const string LoginSecondStepEventualImagePasswordHintFilePath = "/wwwroot/images/blue-hint.jpg";
        public const string LoginSecondStepEventualImagePasswordHintMimeType = "image/jpg";
    }
}
