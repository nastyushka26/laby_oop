using System;
using System.Security.Cryptography;
using System.Text;

namespace PluginInterface
{
    /// <summary>
    /// Plugin signature data structure for integrity and authenticity verification
    /// </summary>
    [Serializable]
    public class PluginSignature
    {
        public string PluginId { get; set; }
        public string PluginName { get; set; }
        public DateTime ExpirationDate { get; set; }
        public string Hash { get; set; }
        public byte[] Signature { get; set; }

        public static string CalculateHash(byte[] pluginBytes)
        {
            using (System.Security.Cryptography.SHA256 sha256 = System.Security.Cryptography.SHA256.Create())
            {
                byte[] hashBytes = sha256.ComputeHash(pluginBytes);
                return Convert.ToBase64String(hashBytes);
            }
        }

        /// <summary>
        /// Create digital signature using RSA
        /// </summary>
        public static byte[] CreateSignature(string data, RSA privateKey)
        {
            byte[] dataBytes = Encoding.UTF8.GetBytes(data);
            return privateKey.SignData(dataBytes, HashAlgorithmName.SHA256, RSASignaturePadding.Pkcs1);
        }

        /// <summary>
        /// Verify digital signature using RSA public key
        /// </summary>
        public static bool VerifySignature(string data, byte[] signature, RSA publicKey)
        {
            byte[] dataBytes = Encoding.UTF8.GetBytes(data);
            return publicKey.VerifyData(dataBytes, signature, HashAlgorithmName.SHA256, RSASignaturePadding.Pkcs1);
        }
    }
}