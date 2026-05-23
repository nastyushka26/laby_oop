using PluginInterface;

namespace StarPlugin
{
    public class StarPluginEntry : IShapePlugin
    {
        private StarFactory _factory;

        public string PluginId => "com.laba4.star.v1";
        public string PluginName => "Star Shape";
        public string Version => "1.0.0";

        public bool Initialize()
        {
            _factory = new StarFactory();
            return true;
        }

        public IShapeFactoryPlugin GetShapeFactory() => _factory;
    }
}