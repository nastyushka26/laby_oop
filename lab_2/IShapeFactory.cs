using laba1oop;
using System.Security.Cryptography.Xml;
using System.Xml.Linq;
using Rectangle = laba1oop.Rectangle;

namespace laba2oop
{
    public interface IShapeFactory
    {
        public void GetCoordinates(int x, int y);
        //public abstract Shape CreateShape();
        public bool IsReady();
        public void Reset();
    }

    public abstract class ShapeBaseFactory : IShapeFactory
    {
        // main fields for all factories
        protected int[,] coordinates;
        protected int index_now = 0;
        protected int need_click_count;
        public string ShapeName;


        // method for array initialization
        public ShapeBaseFactory(int clicksNeeded)
        {
            need_click_count = clicksNeeded;
            coordinates = new int[clicksNeeded, 2];
        }
        // method for getting coordinates into array
        public void GetCoordinates(int x, int y)
        {
            if (index_now < need_click_count)
            {
                coordinates[index_now, 0] = x;
                coordinates[index_now, 1] = y;
                index_now++;
            }
        }

        public abstract Shape CreateShape();

        // method for checking if we can create a shape
        public bool IsReady()
        {
            return need_click_count == index_now;
        }

        public void Reset()
        {
            index_now = 0;
        }
    }

    class CircleFactory : ShapeBaseFactory
    {
        // constructor 
        public CircleFactory(): base(2) // calling ShapeBaseFactory(2)
        {
            ShapeName = "Круг";
        }
        public override Shape CreateShape()
        {
            int x1 = coordinates[0,0], y1 = coordinates[0,1]; 
            int x2 = coordinates[1,0], y2 = coordinates[1,1];
            int radius = (int)Math.Sqrt((x1 - x2) * (x1 - x2) + (y1 - y2) * (y1 - y2));

            return new Circle(x1, y1, radius);
        }
    }

    class LineFactory : ShapeBaseFactory
    {
        public LineFactory() : base(2)
        {
            ShapeName = "Линия";
        }

        public override Shape CreateShape()
        {
            int x1 = coordinates[0,0], y1 = coordinates[0,1];
            int x2 = coordinates[1,0], y2 = coordinates[1,1];

            return new Line(x1, y1, x2, y2);
        }
    }

    class SquareFactory : ShapeBaseFactory
    {
        public SquareFactory() : base(1)
        {
            ShapeName = "Квадрат";
        }
        public override Shape CreateShape()
        {
            int x1 = coordinates[0, 0], y1 = coordinates[0, 1];

            // after click - showing dialog for length input
            string sideStr = Microsoft.VisualBasic.Interaction.InputBox(
                "Enter side length:", "Create Square", "50");

            // if iput - valid number
            if (int.TryParse(sideStr, out int side))
            {
                return new Square(x1 - side / 2, y1 - side / 2, side);
            }

            // if input - unvalid - return default square
            return new Square(x1 - 30, y1 - 30, 60);
        }
    }

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

