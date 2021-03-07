namespace TwoFactorAuth.Services.Contracts
{
    public interface IAppCryptoService
    {
        string EncryptToBase64String(string text);

        string DecryptFromBase64String(string text);
    }
}