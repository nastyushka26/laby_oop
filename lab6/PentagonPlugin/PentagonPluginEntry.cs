using PluginInterface;

namespace PentagonPlugin
{
    public class PentagonPluginEntry : IShapePlugin
    {
        private PentagonFactory _factory;

        public string PluginId => "com.laba4.pentagon.v1";
        public string PluginName => "Pentagon Shape";
        public string Version => "1.0.0";

        public bool Initialize()
        {
            _factory = new PentagonFactory();
            return true;
        }

        public IShapeFactoryPlugin GetShapeFactory() => _factory;
    }
}