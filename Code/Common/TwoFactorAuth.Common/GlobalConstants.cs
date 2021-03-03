namespace TwoFactorAuth.Common
{
    static public class GlobalConstants
    {
        public const string SystemName = "TwoFactorAuth";
        public const string AdministratorRoleName = "Administrator";

        static public class DummyAuthUserSpecs
        {
            public const string Email = "dummy@auth.user.com";

            static public class Passwords
            {
                public const string First = "123";
                public const string Second = "123";
            }
        }
    }
}
