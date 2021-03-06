﻿namespace TwoFactorAuth.Common.Contracts.Configuration
{
    public sealed class AppDummyAuthSpecs
    {
        public XDummyUsers DummyUsers { get; set; }
        public XCookiesSpecs CookiesSettings { get; set; }

        public sealed class XDummyUsers
        {
            public XUser First { get; set; }
            public XUser Second { get; set; }

            public class XUser
            {
                public string Email { get; set; }
                public string Password { get; set; }
            }
        }

        public sealed class XCookiesSpecs
        {
            public int SecondStageEnablingCookieLifespanInMins { get; set; }
            public string CookieNameForEnableAccessToSecondStage { get; set; }
        }
    }
}
