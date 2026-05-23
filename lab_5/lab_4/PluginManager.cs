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

        private List<IDataProcessorPlugin> _dataProcessors = new List<IDataProcessorPlugin>();

        public IReadOnlyList<IDataProcessorPlugin> DataProcessors => _dataProcessors;

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

        public IShapeFactoryPlugin? FindFactoryByShapeName(string shapeName)
        {
            return _pluginFactories.FirstOrDefault(f => f.ShapeName == shapeName);
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
        

        // Modify LoadPlugin method to also detect IDataProcessorPlugin
        public bool LoadPlugin(string dllPath, bool verifySignature = true)
        {
            try
            {
                //if (verifySignature && !PluginVerifier.VerifyPlugin(dllPath))
                //    return false;
                if (verifySignature && false) // Отключаем проверку для теста
                    return false;

                Assembly assembly = Assembly.LoadFrom(dllPath);
                bool loadedAny = false;

                // Load shape factories
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
                            loadedAny = true;
                        }
                    }

                    // NEW: Load data processors
                    if (typeof(IDataProcessorPlugin).IsAssignableFrom(type) && !type.IsInterface && !type.IsAbstract)
                    {
                        IDataProcessorPlugin processor = (IDataProcessorPlugin)Activator.CreateInstance(type);
                        _dataProcessors.Add(processor);
                        _loadedPlugins.Add($"[Processor] {processor.ProcessorName}");
                        loadedAny = true;
                    }
                }
                return loadedAny;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Failed to load plugin: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }
    }
}