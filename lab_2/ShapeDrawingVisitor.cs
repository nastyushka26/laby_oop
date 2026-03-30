namespace laba2oop
{
    public class ShapeDrawingVisitor : IShapeVisitor
    {
        private Graphics _graphics;
        private Pen _pen;

        public ShapeDrawingVisitor(Graphics graphics, Pen pen)
        {
            _graphics = graphics;
            _pen = pen;
        }

        public void Visit(Circle circle)
        {
            _graphics.DrawEllipse(_pen,
                circle.CoordX - circle.Rad,
                circle.CoordY - circle.Rad,
                circle.Rad * 2,
                circle.Rad * 2);
        }

        public void Visit(Rectangle rect)
        {
            _graphics.DrawRectangle(_pen,
                rect.CoordX,
                rect.CoordY,
                rect.width,
                rect.height);
        }

        public void Visit(Square square)
        {
            _graphics.DrawRectangle(_pen,
                square.CoordX,
                square.CoordY,
                square.len,
                square.len);
        }

        public void Visit(Line line)
        {
            _graphics.DrawLine(_pen,
                line.CoordX,
                line.CoordY,
                line.x2,
                line.y2);
        }

        public void Visit(Triangle triangle)
        {
            Point[] points = new Point[]
            {
                new Point(triangle.CoordX, triangle.CoordY),
                new Point(triangle.x2, triangle.y2),
                new Point(triangle.x3, triangle.y3)
            };
            _graphics.DrawPolygon(_pen, points);
        }

        public void Visit(Ellipse ellipse)
        {
            _graphics.DrawEllipse(_pen,
                ellipse.CoordX - ellipse.a,
                ellipse.CoordY - ellipse.b,
                ellipse.a * 2,
                ellipse.b * 2);
        }
    }
}