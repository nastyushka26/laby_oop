// AesEncryptionPlugin.cs
using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using PluginInterface;

namespace AesEncryptionPlugin
{
    public class AesProcessor : IDataProcessorPlugin
    {
        private string _password = "default123";

        public string ProcessorId => "com.laba5.aes.v1";
        public string ProcessorName => "AES Encryption";
        public string Description => "AES-256 encryption (secure)";

        private byte[] GetKey()
        {
            using (var derive = new Rfc2898DeriveBytes(_password, Encoding.UTF8.GetBytes("SaltSaltSalt"), 10000, HashAlgorithmName.SHA256))
            {
                return derive.GetBytes(32);
            }
        }

        private byte[] GetIV()
        {
            using (var derive = new Rfc2898DeriveBytes(_password, Encoding.UTF8.GetBytes("IVSaltIVSalt"), 10000, HashAlgorithmName.SHA256))
            {
                return derive.GetBytes(16);
            }
        }

        public byte[] ProcessBeforeSave(byte[] data)
        {
            using (Aes aes = Aes.Create())
            {
                aes.Key = GetKey();
                aes.IV = GetIV();
                aes.Mode = CipherMode.CBC;
                aes.Padding = PaddingMode.PKCS7;

                using (var ms = new MemoryStream())
                using (var cs = new CryptoStream(ms, aes.CreateEncryptor(), CryptoStreamMode.Write))
                {
                    cs.Write(data, 0, data.Length);
                    cs.FlushFinalBlock();
                    return ms.ToArray();
                }
            }
        }

        public byte[] ProcessAfterLoad(byte[] data)
        {
            try
            {
                using (Aes aes = Aes.Create())
                {
                    aes.Key = GetKey();
                    aes.IV = GetIV();
                    aes.Mode = CipherMode.CBC;
                    aes.Padding = PaddingMode.PKCS7;

                    using (var ms = new MemoryStream(data))
                    using (var cs = new CryptoStream(ms, aes.CreateDecryptor(), CryptoStreamMode.Read))
                    using (var result = new MemoryStream())
                    {
                        cs.CopyTo(result);
                        return result.ToArray();
                    }
                }
            }
            catch
            {
                return data;
            }
        }

        public bool Configure()
        {
            _password = "secure_password_123"; // Example
            return true;
        }

        public IDataProcessorPlugin Clone()
        {
            return new AesProcessor() { _password = this._password };
        }
    }

    public class AesPluginEntry : IShapePlugin
    {
        public string PluginId => "com.laba5.aes.entry";
        public string PluginName => "AES Encryption Processor";
        public string Version => "1.0.0";

        public bool Initialize() => true;
        public IShapeFactoryPlugin GetShapeFactory() => null;
    }
}