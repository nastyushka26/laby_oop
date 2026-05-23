using System;
using System.Drawing;
using System.Reflection;

namespace laba2oop
{
    /// <summary>
    /// Extended visitor that can handle unknown shapes via reflection
    /// </summary>
    public class DynamicShapeVisitor : IShapeVisitor
    {
        private Graphics _graphics;
        private Pen _pen;

        public DynamicShapeVisitor(Graphics graphics, Pen pen)
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

        public void Visit(laba2oop.Rectangle rect)
        {
            _graphics.DrawRectangle(_pen, rect.CoordX, rect.CoordY, rect.width, rect.height);
        }

        public void Visit(Square square)
        {
            _graphics.DrawRectangle(_pen, square.CoordX, square.CoordY, square.len, square.len);
        }

        public void Visit(Line line)
        {
            _graphics.DrawLine(_pen, line.CoordX, line.CoordY, line.x2, line.y2);
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

        /// <summary>
        /// Handle unknown shape types via reflection
        /// </summary>
        /// <summary>
        /// Handle unknown shape types via reflection
        /// </summary>
        public void VisitUnknown(object shape)
        {
            // Try to find GetPoints or GetVertices method
            var getPointsMethod = shape.GetType().GetMethod("GetPoints");
            var getVerticesMethod = shape.GetType().GetMethod("GetVertices");

            if (getPointsMethod != null)
            {
                var points = getPointsMethod.Invoke(shape, null) as Point[];
                if (points != null && points.Length > 0)
                    _graphics.DrawPolygon(_pen, points);
            }
            else if (getVerticesMethod != null)
            {
                var vertices = getVerticesMethod.Invoke(shape, null) as Point[];
                if (vertices != null && vertices.Length > 0)
                    _graphics.DrawPolygon(_pen, vertices);
            }
        }
    }
}