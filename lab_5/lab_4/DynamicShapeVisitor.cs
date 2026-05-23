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
        public void VisitUnknown(object shape)
        {
            if (shape == null) return;

            // Try to find GetPoints or GetVertices method
            var getPointsMethod = shape.GetType().GetMethod("GetPoints");
            var getVerticesMethod = shape.GetType().GetMethod("GetVertices");

            // Also check for properties that might contain points
            var pointsProp = shape.GetType().GetProperty("Points");
            var verticesProp = shape.GetType().GetProperty("Vertices");

            Point[] points = null;

            if (getPointsMethod != null)
            {
                var result = getPointsMethod.Invoke(shape, null);
                points = result as Point[];
            }
            else if (getVerticesMethod != null)
            {
                var result = getVerticesMethod.Invoke(shape, null);
                points = result as Point[];
            }
            else if (pointsProp != null)
            {
                points = pointsProp.GetValue(shape) as Point[];
            }
            else if (verticesProp != null)
            {
                points = verticesProp.GetValue(shape) as Point[];
            }

            if (points != null && points.Length > 0)
            {
                _graphics.DrawPolygon(_pen, points);
            }
            else
            {
                // Try to draw as ellipse based on properties
                var centerXProp = shape.GetType().GetProperty("CenterX");
                var centerYProp = shape.GetType().GetProperty("CenterY");
                var outerRadiusProp = shape.GetType().GetProperty("OuterRadius");
                var radiusProp = shape.GetType().GetProperty("Radius");

                if (centerXProp != null && centerYProp != null)
                {
                    int cx = Convert.ToInt32(centerXProp.GetValue(shape));
                    int cy = Convert.ToInt32(centerYProp.GetValue(shape));

                    if (outerRadiusProp != null)
                    {
                        int r = Convert.ToInt32(outerRadiusProp.GetValue(shape));
                        _graphics.DrawEllipse(_pen, cx - r, cy - r, r * 2, r * 2);
                    }
                    else if (radiusProp != null)
                    {
                        int r = Convert.ToInt32(radiusProp.GetValue(shape));
                        _graphics.DrawEllipse(_pen, cx - r, cy - r, r * 2, r * 2);
                    }
                    else
                    {
                        // Draw a simple cross at center
                        _graphics.DrawLine(_pen, cx - 10, cy, cx + 10, cy);
                        _graphics.DrawLine(_pen, cx, cy - 10, cx, cy + 10);
                    }
                }
            }
        }
    }
}