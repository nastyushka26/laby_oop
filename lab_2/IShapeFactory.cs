using System.Security.Cryptography.Xml;
using System.Xml.Linq;
using Rectangle = laba2oop.Rectangle;

namespace laba2oop
{
    // creating interface f or shape factories classes
    public interface IShapeFactory
    {
        // method for getting coordinates into coordinates array
        public void GetCoordinates(int x, int y);
        // method for checking if we can create a shape
        public bool IsReady();
        // method for reseting number of clickes that been already done
        public void Reset();
        // Creates a shape using the collected coordinates
        public Shape CreateShape();
    }

    // Abstract base class implementing common functionality for all factories
    public abstract class ShapeBaseFactory : IShapeFactory
    {
        // main fields for all factories
        protected int[,] coordinates;
        public int index_now = 0; // clicks already done
        public int need_click_count; // needed count of clicks
        public string ShapeName;

        // method for coordinates array initialization
        public ShapeBaseFactory(int clicksNeeded)
        {
            need_click_count = clicksNeeded;
            coordinates = new int[clicksNeeded, 2];
        }
        // method for storing mouse click coordinates if more clicks are needed
        public void GetCoordinates(int x, int y)
        {
            if (index_now < need_click_count)
            {
                coordinates[index_now, 0] = x;
                coordinates[index_now, 1] = y;
                index_now++;
            }
        }
        // Abstract method to be implemented by specific factories
        public abstract Shape CreateShape();

        // method for checking if we can create a shape
        public bool IsReady()
        {
            return need_click_count == index_now;
        }
        // method for reseting number of clickes that been already done
        public void Reset()
        {
            index_now = 0;
        }
    }

    // Factory for creating Circle objects
    // Requires 2 clicks: first for center, second for radius point
    class CircleFactory : ShapeBaseFactory
    {
        // constructor 
        public CircleFactory(): base(2) // calling ShapeBaseFactory(2)
        {
            ShapeName = "Круг";
        }
        // Creates a circle using two click points
        public override Shape CreateShape()
        {
            int x1 = coordinates[0,0], y1 = coordinates[0,1]; 
            int x2 = coordinates[1,0], y2 = coordinates[1,1];
            int radius = (int)Math.Sqrt((x1 - x2) * (x1 - x2) + (y1 - y2) * (y1 - y2));

            return new Circle(x1, y1, radius);
        }
    }

    // Factory for creating Line objects
    // Requires 2 clicks: start point and end point
    class LineFactory : ShapeBaseFactory
    {
        public LineFactory() : base(2)
        {
            ShapeName = "Линия";
        }

        // Creates a line from first click to second click
        public override Shape CreateShape()
        {
            int x1 = coordinates[0,0], y1 = coordinates[0,1];
            int x2 = coordinates[1,0], y2 = coordinates[1,1];

            return new Line(x1, y1, x2, y2);
        }
    }

    // Factory for creating Square objects
    // Requires 1 click for center, then asks for side length in dialog
    class SquareFactory : ShapeBaseFactory
    {
        public SquareFactory() : base(1)
        {
            ShapeName = "Квадрат";
        }
        // Creates a square using center click and side length from dialog
        public override Shape CreateShape()
        {
            int x1 = coordinates[0, 0], y1 = coordinates[0, 1];

            // after click - showing dialog for length input
            string sideStr = Microsoft.VisualBasic.Interaction.InputBox(
                "Enter side length:", "Create Square", "50");

            // if iput - valid number
            if (int.TryParse(sideStr, out int side))
            {
                // Calculate top-left corner from center
                return new Square(x1 - side / 2, y1 - side / 2, side);
            }

            // if input - unvalid - return default square
            return new Square(x1 - 30, y1 - 30, 60);
        }
    }

    // Factory for creating Ellipse objects
    // Requires 1 click for center, then asks for semi-axis length in dialog
    class EllipseFactory : ShapeBaseFactory
    {
        public EllipseFactory() : base(1)
        {
            ShapeName = "Овал";
        }
        public override Shape CreateShape()
        {
            int x1 = coordinates[0,0], y1 = coordinates[0,1];
            int a, b;

            // diaolg for semi-axis iput
            string aStr = Microsoft.VisualBasic.Interaction.InputBox(
                "Enter semi-axis a (width):", "Create Ellipse", "50");
            string bStr = Microsoft.VisualBasic.Interaction.InputBox(
                "Enter semi-axis b (height):", "Create Ellipse", "30");

            if (int.TryParse(aStr, out a) && int.TryParse(bStr, out b))
            {
                // valid input - returning ellipse
                return new Ellipse(x1, y1, a, b);
            }

            // return default ellipse
            return new Ellipse(x1, y1, 50, 30);
        }
    }

    class RectangleFactory : ShapeBaseFactory
    {
        public RectangleFactory() : base(2) // 2 clicks needed
        {
            ShapeName = "Прямоугольник";
        }
        public override Shape CreateShape()
        {
            int x1 = coordinates[0, 0], y1 = coordinates[0, 1];
            int x2 = coordinates[1, 0], y2 = coordinates[1, 1];
            int width = Math.Abs(x1 - x2);
            int height = Math.Abs(y1 - y2);

            return new Rectangle(Math.Min(x1, x2), Math.Min(y1, y2), width, height);
        }
    }

    class TriangleFactory : ShapeBaseFactory
    {
        public TriangleFactory() : base(3)
        {
            ShapeName = "Треугольник";
        }
        public override Shape CreateShape()
        {
            int x1 = coordinates[0, 0], y1 = coordinates[0, 1];
            int x2 = coordinates[1, 0], y2 = coordinates[1, 1];
            int x3 = coordinates[2, 0], y3 = coordinates[2, 1];

            return new Triangle(x1, y1, x2, y2, x3, y3);
        }
    }
}

