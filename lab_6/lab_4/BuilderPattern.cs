// BuilderPattern.cs
using System.Drawing;

namespace laba2oop
{
    /// <summary>
    /// Builder Pattern - Separates the construction of a complex object
    /// from its representation, allowing the same construction process
    /// to create different representations.
    /// Used for creating complex shapes with multiple parameters.
    /// </summary>

    // Product
    public class ComplexShape
    {
        public List<Shape> Shapes { get; } = new List<Shape>();
        public string Name { get; set; } = "";
        public Color OutlineColor { get; set; } = Color.Black;

        public void AddShape(Shape shape) => Shapes.Add(shape);

        public void Draw(Graphics g, Pen pen)
        {
            var visitor = new DynamicShapeVisitor(g, pen);
            foreach (var shape in Shapes)
            {
                shape.Accept(visitor);
            }
        }
    }

    // Builder interface
    public interface IShapeBuilder
    {
        void Reset();
        void SetName(string name);
        void SetOutlineColor(Color color);
        void AddShape(Shape shape);
        ComplexShape GetResult();
    }

    // Concrete Builder
    public class ComplexShapeBuilder : IShapeBuilder
    {
        private ComplexShape _complexShape = new ComplexShape();

        public void Reset() => _complexShape = new ComplexShape();

        public void SetName(string name) => _complexShape.Name = name;

        public void SetOutlineColor(Color color) => _complexShape.OutlineColor = color;

        public void AddShape(Shape shape) => _complexShape.AddShape(shape);

        public ComplexShape GetResult()
        {
            var result = _complexShape;
            Reset();
            return result;
        }
    }

    // Director for building predefined complex shapes
    public class ShapeBuilderDirector
    {
        private IShapeBuilder _builder;

        public ShapeBuilderDirector(IShapeBuilder builder)
        {
            _builder = builder;
        }

        // Build a simple house: square + triangle roof
        public ComplexShape BuildHouse(int x, int y)
        {
            _builder.Reset();
            _builder.SetName("House");

            // Square body
            var square = new Square(x, y, 60);
            _builder.AddShape(square);

            // Triangle roof
            var roof = new Triangle(x, y, x + 30, y - 40, x + 60, y);
            _builder.AddShape(roof);

            return _builder.GetResult();
        }

        // Build a smiley face: circle + eyes
        public ComplexShape BuildSmileyFace(int x, int y)
        {
            _builder.Reset();
            _builder.SetName("Smiley Face");

            // Face circle
            var face = new Circle(x, y, 50);
            _builder.AddShape(face);

            // Left eye
            var leftEye = new Circle(x - 20, y - 20, 5);
            _builder.AddShape(leftEye);

            // Right eye
            var rightEye = new Circle(x + 20, y - 20, 5);
            _builder.AddShape(rightEye);

            // Smile (using line for simplicity - can be arc for better effect)
            var smile = new Line(x - 25, y + 10, x + 25, y + 10);
            _builder.AddShape(smile);

            return _builder.GetResult();
        }

        // Build a flower: circle center + petals
        public ComplexShape BuildFlower(int x, int y)
        {
            _builder.Reset();
            _builder.SetName("Flower");

            // Center
            var center = new Circle(x, y, 15);
            _builder.AddShape(center);

            // Petals
            int[] angles = { 0, 60, 120, 180, 240, 300 };
            foreach (var angle in angles)
            {
                double rad = angle * Math.PI / 180;
                int petalX = x + (int)(30 * Math.Cos(rad));
                int petalY = y + (int)(30 * Math.Sin(rad));
                var petal = new Ellipse(petalX, petalY, 15, 25);
                _builder.AddShape(petal);
            }

            return _builder.GetResult();
        }
    }
}