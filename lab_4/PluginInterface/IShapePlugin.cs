using System;

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
    }

    
}