namespace TwoFactorAuth.Common.Contracts
{
    static public class GlobalConstants
    {
        public const string SystemName = "TwoFactorAuth";
        public const string AdministratorRoleName = "Administrator";

        public const string LoginSecondStepImageHintActionName = "password-hint.jpg"; //for the controller
        public const string LoginSecondStepBaseImagePasswordHintFilePath = "/Content/BlueHint.orig.jpg";
        public const string LoginSecondStepEventualImagePasswordHintFilePath = "/Content/BlueHint.jpg";
        public const string LoginSecondStepEventualImagePasswordHintMimeType = "image/jpg";
    }
}

