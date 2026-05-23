using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using PluginInterface;

namespace PluginSigner
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length < 1)
            {
                Console.WriteLine("Usage: PluginSigner <plugin.dll>");
                Console.WriteLine("Example: PluginSigner StarPlugin.dll");
                return;
            }

            string pluginPath = args[0];
            if (!File.Exists(pluginPath))
            {
                Console.WriteLine($"File not found: {pluginPath}");
                return;
            }

            SignPlugin(pluginPath);
        }

        static void SignPlugin(string pluginPath)
        {
            Console.WriteLine($"Signing plugin: {pluginPath}");

            // Generate RSA key pair
            using (RSA rsa = RSA.Create(2048))
            {
                // Save keys
                string publicKeyPath = "public_key.xml";
                string privateKeyPath = "private_key.xml";

                if (!File.Exists(privateKeyPath))
                {
                    File.WriteAllText(privateKeyPath, rsa.ToXmlString(true));
                    File.WriteAllText(publicKeyPath, rsa.ToXmlString(false));
                    Console.WriteLine($"Generated new key pair: {privateKeyPath}, {publicKeyPath}");
                }
                else
                {
                    rsa.FromXmlString(File.ReadAllText(privateKeyPath));
                }

                // Read and hash plugin
                byte[] pluginBytes = File.ReadAllBytes(pluginPath);
                string hash = PluginSignature.CalculateHash(pluginBytes);
                DateTime expiration = DateTime.Now.AddMonths(6);

                // Create signature
                string signatureData = $"{Path.GetFileName(pluginPath)}|{hash}|{expiration.Ticks}";
                byte[] signature = SignData(signatureData, rsa);

                // Save signature
                var sig = new PluginSignature
                {
                    PluginId = Path.GetFileName(pluginPath),
                    PluginName = Path.GetFileNameWithoutExtension(pluginPath),
                    ExpirationDate = expiration,
                    Hash = hash,
                    Signature = signature
                };

                string sigPath = pluginPath + ".sig";
                string json = JsonSerializer.Serialize(sig);
                File.WriteAllText(sigPath, json);

                Console.WriteLine($"Signature created: {sigPath}");
                Console.WriteLine($"Expires: {expiration}");
                Console.WriteLine($"Hash: {hash}");
            }
        }

        static byte[] SignData(string data, RSA rsa)
        {
            byte[] dataBytes = Encoding.UTF8.GetBytes(data);
            return rsa.SignData(dataBytes, HashAlgorithmName.SHA256, RSASignaturePadding.Pkcs1);
        }
    }

    // Extension method for RSA XML serialization
    public static class RSAExtensions
    {
        public static void FromXmlString(this RSA rsa, string xmlString)
        {
            var parameters = new RSAParameters();
            System.Xml.XmlDocument doc = new System.Xml.XmlDocument();
            doc.LoadXml(xmlString);

            if (doc.DocumentElement.SelectSingleNode("Modulus") is System.Xml.XmlNode modulus)
                parameters.Modulus = Convert.FromBase64String(modulus.InnerText);
            if (doc.DocumentElement.SelectSingleNode("Exponent") is System.Xml.XmlNode exponent)
                parameters.Exponent = Convert.FromBase64String(exponent.InnerText);
            if (doc.DocumentElement.SelectSingleNode("P") is System.Xml.XmlNode p)
                parameters.P = Convert.FromBase64String(p.InnerText);
            if (doc.DocumentElement.SelectSingleNode("Q") is System.Xml.XmlNode q)
                parameters.Q = Convert.FromBase64String(q.InnerText);
            if (doc.DocumentElement.SelectSingleNode("DP") is System.Xml.XmlNode dp)
                parameters.DP = Convert.FromBase64String(dp.InnerText);
            if (doc.DocumentElement.SelectSingleNode("DQ") is System.Xml.XmlNode dq)
                parameters.DQ = Convert.FromBase64String(dq.InnerText);
            if (doc.DocumentElement.SelectSingleNode("InverseQ") is System.Xml.XmlNode inverseQ)
                parameters.InverseQ = Convert.FromBase64String(inverseQ.InnerText);
            if (doc.DocumentElement.SelectSingleNode("D") is System.Xml.XmlNode d)
                parameters.D = Convert.FromBase64String(d.InnerText);

            rsa.ImportParameters(parameters);
        }

        public static string ToXmlString(this RSA rsa, bool includePrivateParameters)
        {
            RSAParameters parameters = rsa.ExportParameters(includePrivateParameters);

            return $@"<RSAKeyValue>
                <Modulus>{Convert.ToBase64String(parameters.Modulus)}</Modulus>
                <Exponent>{Convert.ToBase64String(parameters.Exponent)}</Exponent>
                {(includePrivateParameters && parameters.P != null ? $"<P>{Convert.ToBase64String(parameters.P)}</P>" : "")}
                {(includePrivateParameters && parameters.Q != null ? $"<Q>{Convert.ToBase64String(parameters.Q)}</Q>" : "")}
                {(includePrivateParameters && parameters.DP != null ? $"<DP>{Convert.ToBase64String(parameters.DP)}</DP>" : "")}
                {(includePrivateParameters && parameters.DQ != null ? $"<DQ>{Convert.ToBase64String(parameters.DQ)}</DQ>" : "")}
                {(includePrivateParameters && parameters.InverseQ != null ? $"<InverseQ>{Convert.ToBase64String(parameters.InverseQ)}</InverseQ>" : "")}
                {(includePrivateParameters && parameters.D != null ? $"<D>{Convert.ToBase64String(parameters.D)}</D>" : "")}
                </RSAKeyValue>";
        }
    }
}