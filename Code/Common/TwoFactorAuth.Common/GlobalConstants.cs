namespace TwoFactorAuth.Common
{
    static public class GlobalConstants
    {
        public const string SystemName = "TwoFactorAuth";
        public const string AdministratorRoleName = "Administrator";
        
        static public class DummyAuthSpecs
        {
            public const string Email = "dummy@auth.user.com";

            static public class Passwords
            {
                public const string First = "123";
                public const string Second = "blue1sF0rev3r";
            }

            static public class CookieSpecs
            {
                public const string EnableAccessToSecondStage = ".AspNet.DummyAuth.SecondStageAccessEnabled";

                public const int SecondStageEnablingCookieLifespanInMins = 2 * 60; //2 hours
            }
        }
    }
}
