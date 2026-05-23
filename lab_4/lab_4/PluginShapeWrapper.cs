using System;
using System.Drawing;

namespace laba2oop
{
    /// <summary>
    /// Wrapper for plugin shapes to integrate with the main app's visitor pattern
    /// </summary>
    public class PluginShapeWrapper : Shape
    {
        private object _wrappedShape;
        private string _shapeType;

        public PluginShapeWrapper(object wrappedShape, string shapeType) : base(0, 0)
        {
            _wrappedShape = wrappedShape;
            _shapeType = shapeType;
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
            // Просто передаем в DynamicShapeVisitor
            if (visitor is DynamicShapeVisitor dynamicVisitor)
            {
                dynamicVisitor.VisitUnknown(_wrappedShape);
            }
        }

        public object WrappedShape => _wrappedShape;
        public string ShapeType => _shapeType;
    }
}