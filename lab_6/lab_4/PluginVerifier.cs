using System;
using System.IO;
using System.Security.Cryptography;
using System.Text.Json;
using PluginInterface;

namespace laba2oop
{
    /// <summary>
    /// Verifies plugin signatures for authenticity and integrity
    /// </summary>
    public static class PluginVerifier
    {
        private static RSA _publicKey;

        static PluginVerifier()
        {
            LoadPublicKey();
        }

        /// <summary>
        /// Load public key from embedded resource or file
        /// </summary>
        private static void LoadPublicKey()
        {
            try
            {
                string publicKeyPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "public_key.xml");
                if (File.Exists(publicKeyPath))
                {
                    _publicKey = RSA.Create();
                    _publicKey.FromXmlString(File.ReadAllText(publicKeyPath));
                }
                else
                {
                    // In production, embed the public key in the application
                    // For demo, we'll accept unsigned plugins or create a default
                    _publicKey = null;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed to load public key: {ex.Message}");
                _publicKey = null;
            }
        }

        /// <summary>
        /// Verify plugin signature
        /// </summary>
        public static bool VerifyPlugin(string pluginPath)
        {
            string signaturePath = pluginPath + ".sig";

            if (!File.Exists(signaturePath))
            {
                // No signature file - ask user if they trust this plugin
                var result = System.Windows.Forms.MessageBox.Show(
                    $"Plugin '{Path.GetFileName(pluginPath)}' is not signed.\n\nDo you want to load it anyway?",
                    "Unsigned Plugin",
                    System.Windows.Forms.MessageBoxButtons.YesNo,
                    System.Windows.Forms.MessageBoxIcon.Warning);
                return result == System.Windows.Forms.DialogResult.Yes;
            }

            try
            {
                // Read signature
                string json = File.ReadAllText(signaturePath);
                PluginSignature signature = JsonSerializer.Deserialize<PluginSignature>(json);

                // Read plugin bytes
                byte[] pluginBytes = File.ReadAllBytes(pluginPath);
                string currentHash = PluginSignature.CalculateHash(pluginBytes);

                // Check integrity
                if (currentHash != signature.Hash)
                {
                    System.Windows.Forms.MessageBox.Show(
                        $"Plugin '{Path.GetFileName(pluginPath)}' has been modified!",
                        "Integrity Check Failed",
                        System.Windows.Forms.MessageBoxButtons.OK,
                        System.Windows.Forms.MessageBoxIcon.Error);
                    return false;
                }

                // Check expiration
                if (DateTime.Now > signature.ExpirationDate)
                {
                    System.Windows.Forms.MessageBox.Show(
                        $"Plugin '{Path.GetFileName(pluginPath)}' has expired!",
                        "Plugin Expired",
                        System.Windows.Forms.MessageBoxButtons.OK,
                        System.Windows.Forms.MessageBoxIcon.Warning);
                    return false;
                }

                // Verify signature if we have public key
                if (_publicKey != null)
                {
                    string signatureData = $"{Path.GetFileName(pluginPath)}|{signature.Hash}|{signature.ExpirationDate.Ticks}";
                    bool isValid = PluginSignature.VerifySignature(signatureData, signature.Signature, _publicKey);

                    if (!isValid)
                    {
                        System.Windows.Forms.MessageBox.Show(
                            $"Plugin '{Path.GetFileName(pluginPath)}' has invalid signature!",
                            "Signature Check Failed",
                            System.Windows.Forms.MessageBoxButtons.OK,
                            System.Windows.Forms.MessageBoxIcon.Error);
                        return false;
                    }
                }

                return true;
            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show(
                    $"Failed to verify plugin '{Path.GetFileName(pluginPath)}': {ex.Message}",
                    "Verification Error",
                    System.Windows.Forms.MessageBoxButtons.OK,
                    System.Windows.Forms.MessageBoxIcon.Error);
                return false;
            }
        }
    }
}