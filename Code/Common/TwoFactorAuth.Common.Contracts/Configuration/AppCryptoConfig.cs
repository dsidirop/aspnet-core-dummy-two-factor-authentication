namespace TwoFactorAuth.Common.Contracts.Configuration
{
    public sealed class AppCryptoConfig
    {
        public string IV { get; set; }
        public string Key { get; set; }
    }
}
