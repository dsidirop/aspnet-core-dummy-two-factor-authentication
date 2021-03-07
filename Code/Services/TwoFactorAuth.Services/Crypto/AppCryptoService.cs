using TwoFactorAuth.Services.Contracts;

namespace TwoFactorAuth.Services.Crypto
{
    using System;
    using System.Security.Cryptography;
    using System.Text;

    using Microsoft.Extensions.Options;

    using TwoFactorAuth.Common.Configuration;

    public class AppCryptoService : IAppCryptoService
    {
        private readonly RijndaelManaged _rijndael;
        
        public AppCryptoService(IOptionsMonitor<AppCryptoConfig> optionsMonitor) //0
        {
            var options = optionsMonitor.CurrentValue;

            _rijndael = new RijndaelManaged
            {
                Mode = CipherMode.CBC,
                Padding = PaddingMode.PKCS7,

                IV = Convert.FromBase64String(Convert.ToBase64String(Encoding.UTF8.GetBytes(options.IV))),
                Key = Convert.FromBase64String(Convert.ToBase64String(Encoding.UTF8.GetBytes(options.Key))),
            };

            //0 https://medium.com/@dozieogbo/a-better-way-to-inject-appsettings-in-asp-net-core-96be36ffa22b
        }
        
        #region public
        
        public string EncryptToBase64String(string text)
        {
            text = text?.Trim();

            return Convert.ToBase64String(EncryptToByte(text));
        }
        
        public string DecryptFromBase64String(string text)
        {
            return Decrypt(Convert.FromBase64String(text));
        }

        #endregion


        #region private

        private byte[] EncryptToByte(string plain)
        {
            var cipher = UTF8Encoding.GetBytes(plain);

            var encryptedValue = _rijndael
                .CreateEncryptor()
                .TransformFinalBlock(
                    inputBuffer: cipher,
                    inputOffset: 0,
                    inputCount: cipher.Length
                );

            return encryptedValue;
        }

        public string Decrypt(byte[] cipher)
        {
            var decryptedValue = _rijndael
                .CreateDecryptor()
                .TransformFinalBlock(
                    inputBuffer: cipher,
                    inputOffset: 0,
                    inputCount: cipher.Length
                );

            return UTF8Encoding.GetString(decryptedValue);
        }

        static private readonly UTF8Encoding UTF8Encoding = new();

        #endregion
    }
}
