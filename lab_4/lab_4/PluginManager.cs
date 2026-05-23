using PluginInterface;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.Json;
using System.Windows.Forms;

namespace laba2oop
{
    public class PluginManager
    {
        private List<IShapeFactoryPlugin> _pluginFactories = new List<IShapeFactoryPlugin>();
        private List<string> _loadedPlugins = new List<string>();

        public IReadOnlyList<IShapeFactoryPlugin> PluginFactories => _pluginFactories;
        public IReadOnlyList<string> LoadedPlugins => _loadedPlugins;

        public int LoadPluginsFromDirectory(string directory, bool verifySignature = true)
        {
            int loadedCount = 0;
            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
                return 0;
            }

            foreach (string dllPath in Directory.GetFiles(directory, "*.dll"))
            {
                if (Path.GetFileName(dllPath).Contains("PluginInterface"))
                    continue;

                if (LoadPlugin(dllPath, verifySignature))
                    loadedCount++;
            }
            return loadedCount;
        }

        public bool LoadPlugin(string dllPath, bool verifySignature = true)
        {
            try
            {
                if (verifySignature && !PluginVerifier.VerifyPlugin(dllPath))
                {
                    return false;
                }

                Assembly assembly = Assembly.LoadFrom(dllPath);

                foreach (Type type in assembly.GetExportedTypes())
                {
                    if (typeof(IShapePlugin).IsAssignableFrom(type) && !type.IsInterface && !type.IsAbstract)
                    {
                        IShapePlugin plugin = (IShapePlugin)Activator.CreateInstance(type);
                        if (plugin.Initialize())
                        {
                            _loadedPlugins.Add(plugin.PluginName);
                            IShapeFactoryPlugin factory = plugin.GetShapeFactory();
                            if (factory != null)
                                _pluginFactories.Add(factory);
                            return true;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Failed to load plugin: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            return false;
        }

        // ДОБАВЛЕННЫЙ МЕТОД
        public bool LoadPluginFromArgument(string pluginName)
        {
            string pluginPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Plugins", pluginName);
            if (!pluginPath.EndsWith(".dll"))
                pluginPath += ".dll";

            if (File.Exists(pluginPath))
                return LoadPlugin(pluginPath);

            return false;
        }

        // ДОБАВЛЕННЫЙ МЕТОД
        public PluginFactoryAdapter CreateFactoryAdapter(IShapeFactoryPlugin pluginFactory)
        {
            return new PluginFactoryAdapter(pluginFactory);
        }

        private bool VerifyPluginSignature(string dllPath)
        {
            string sigPath = dllPath + ".sig";
            if (!File.Exists(sigPath))
            {
                DialogResult result = MessageBox.Show(
                    $"Plugin '{Path.GetFileName(dllPath)}' is not signed.\nLoad anyway?",
                    "Unsigned Plugin", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                return result == DialogResult.Yes;
            }

            try
            {
                string json = File.ReadAllText(sigPath);
                PluginSignature sig = JsonSerializer.Deserialize<PluginSignature>(json);
                byte[] pluginBytes = File.ReadAllBytes(dllPath);
                string currentHash = PluginSignature.CalculateHash(pluginBytes);

                if (currentHash != sig.Hash)
                {
                    MessageBox.Show("Plugin integrity check failed!", "Error",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }

                if (DateTime.Now > sig.ExpirationDate)
                {
                    MessageBox.Show("Plugin has expired!", "Error",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return false;
                }

                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Verification error: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }
    }
}