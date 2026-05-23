// IShapePlugin.cs
using System;
using System.Collections.Generic;

namespace PluginInterface
{
    public interface IShapePlugin
    {
        string PluginId { get; }
        string PluginName { get; }
        string Version { get; }
        bool Initialize();
        IShapeFactoryPlugin GetShapeFactory();
    }

    public interface IShapeFactoryPlugin
    {
        string ShapeName { get; }
        int NeededClicks { get; }
        void AddClick(int x, int y);
        bool IsReady();
        object CreateShape();
        void Reset();
        (int X, int Y)[] GetCoordinates();

        /// <summary>
        /// Serialize shape to string for saving
        /// </summary>
        string SerializeShape(object shape);

        /// <summary>
        /// Deserialize shape from string after loading
        /// </summary>
        object DeserializeShape(string data, int defaultX, int defaultY);
    }

    /// <summary>
    /// Interface for encryption/processing plugins
    /// </summary>
    public interface IDataProcessorPlugin
    {
        string ProcessorId { get; }
        string ProcessorName { get; }
        string Description { get; }

        /// <summary>
        /// Process data before save (encrypt/transform)
        /// </summary>
        byte[] ProcessBeforeSave(byte[] data);

        /// <summary>
        /// Process data after load (decrypt/restore)
        /// </summary>
        byte[] ProcessAfterLoad(byte[] data);

        /// <summary>
        /// Show configuration dialog
        /// </summary>
        bool Configure();

        /// <summary>
        /// Clone for settings (to keep original defaults)
        /// </summary>
        IDataProcessorPlugin Clone();
    }
}