namespace TwoFactorAuth.Services.Crypto
{
    public interface IAppCryptoService
    {
        string EncryptToBase64String(string text);

        string DecryptFromBase64String(string text);
    }
}