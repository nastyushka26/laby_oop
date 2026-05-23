using System.Collections.Generic;
using PluginInterface;

namespace laba2oop
{
    public class PluginFactoryAdapter : IShapeFactory
    {
        private IShapeFactoryPlugin _pluginFactory;
        private List<(int X, int Y)> _clicks = new List<(int, int)>();

        public PluginFactoryAdapter(IShapeFactoryPlugin pluginFactory)
        {
            _pluginFactory = pluginFactory;
        }

        public string ShapeName => _pluginFactory.ShapeName;
        public int need_click_count => _pluginFactory.NeededClicks;
        public int index_now => _clicks.Count;

        public void GetCoordinates(int x, int y)
        {
            _clicks.Add((x, y));
            _pluginFactory.AddClick(x, y);
        }

        public bool IsReady() => _pluginFactory.IsReady();

        public void Reset()
        {
            _clicks.Clear();
            _pluginFactory.Reset();
        }

        public Shape CreateShape()
        {
            object shapeObject = _pluginFactory.CreateShape();
            return new PluginShapeWrapper(shapeObject, _pluginFactory.ShapeName, _pluginFactory);
        }

        /// <summary>
        /// Get the underlying plugin factory
        /// </summary>
        public IShapeFactoryPlugin GetPluginFactory() => _pluginFactory;
    }
}