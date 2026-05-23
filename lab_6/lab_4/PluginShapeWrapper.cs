using System;
using System.Drawing;
using PluginInterface;

namespace laba2oop
{
    /// <summary>
    /// Wrapper for plugin shapes to integrate with the main app's visitor pattern
    /// </summary>
    public class PluginShapeWrapper : Shape
    {
        private object _wrappedShape;
        private string _shapeType;
        private IShapeFactoryPlugin? _sourceFactory;

        public PluginShapeWrapper(object wrappedShape, string shapeType, IShapeFactoryPlugin? sourceFactory = null) : base(0, 0)
        {
            _wrappedShape = wrappedShape;
            _shapeType = shapeType;
            _sourceFactory = sourceFactory;
            ExtractCoordinates();
        }

        private void ExtractCoordinates()
        {
            var props = _wrappedShape.GetType().GetProperties();
            foreach (var prop in props)
            {
                if (prop.Name == "CenterX" || prop.Name == "X")
                    CoordX = (int)prop.GetValue(_wrappedShape);
                if (prop.Name == "CenterY" || prop.Name == "Y")
                    CoordY = (int)prop.GetValue(_wrappedShape);
            }
        }

        public override void Accept(IShapeVisitor visitor)
        {
            if (visitor is DynamicShapeVisitor dynamicVisitor)
            {
                dynamicVisitor.VisitUnknown(_wrappedShape);
            }
        }

        public object WrappedShape => _wrappedShape;
        public string ShapeType => _shapeType;
        public IShapeFactoryPlugin? SourceFactory => _sourceFactory;

        /// <summary>
        /// Serialize this plugin shape using its factory
        /// </summary>
        public string Serialize()
        {
            if (_sourceFactory != null)
            {
                return _sourceFactory.SerializeShape(_wrappedShape);
            }
            return "{}";
        }
    }
}