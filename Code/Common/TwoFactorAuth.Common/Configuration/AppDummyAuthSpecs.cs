namespace TwoFactorAuth.Common.Configuration
{
    public class AppDummyAuthSpecs
    {
        public string EmailFirstDummyAuthUser { get; set; }
        public string EmailEventualDummyAuthUser { get; set; }

        public XPasswords Passwords { get; set; }
        public XCookiesSpecs CookiesSettings { get; set; }

        public class XPasswords
        {
            public string First { get; set; }
            public string Second { get; set; }
        }

        public class XCookiesSpecs
        {
            public int SecondStageEnablingCookieLifespanInMins { get; set; }
            public string CookieNameForEnableAccessToSecondStage { get; set; }
        }
    }
}