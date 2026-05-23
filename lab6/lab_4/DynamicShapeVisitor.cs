// DynamicShapeVisitor.cs
using System;
using System.Drawing;
using System.Reflection;
using System.Linq;

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


        // Добавьте этот метод в класс DynamicShapeVisitor:
        /// <summary>
        /// Draw heart shape correctly using friend's GetHeartPoints method
        /// </summary>
        private void DrawHeartCorrectly(object heart)
        {
            try
            {
                // Try to get heart points via GetHeartPoints method
                var getPointsMethod = heart.GetType().GetMethod("GetHeartPoints");
                if (getPointsMethod != null)
                {
                    var result = getPointsMethod.Invoke(heart, null);
                    if (result is PointF[] pointsF && pointsF.Length > 0)
                    {
                        // Convert PointF[] to Point[] for drawing
                        Point[] points = pointsF.Select(p => new Point((int)p.X, (int)p.Y)).ToArray();

                        // Fill with red color
                        using (SolidBrush brush = new SolidBrush(Color.Red))
                        {
                            _graphics.FillPolygon(brush, points);
                        }
                        _graphics.DrawPolygon(_pen, points);
                        return;
                    }
                }

                // Fallback: get Width, Height and Location properties
                var widthProp = heart.GetType().GetProperty("Width");
                var heightProp = heart.GetType().GetProperty("Height");
                var locationProp = heart.GetType().GetProperty("Location");

                int width = 60, height = 55;
                int cx = 100, cy = 100;

                if (widthProp != null) width = Convert.ToInt32(widthProp.GetValue(heart));
                if (heightProp != null) height = Convert.ToInt32(heightProp.GetValue(heart));

                if (locationProp != null)
                {
                    var location = locationProp.GetValue(heart);
                    var xProp = location?.GetType().GetProperty("X");
                    var yProp = location?.GetType().GetProperty("Y");
                    if (xProp != null) cx = Convert.ToInt32(xProp.GetValue(location));
                    if (yProp != null) cy = Convert.ToInt32(yProp.GetValue(location));
                }

                // Draw simple heart using ellipses and triangle
                DrawSimpleHeart(cx, cy, width, height);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"DrawHeartCorrectly error: {ex.Message}");
                DrawSimpleHeart(100, 100, 60, 55);
            }
        }

        /// <summary>
        /// Draw simple heart shape
        /// </summary>
        private void DrawSimpleHeart(int cx, int cy, int width, int height)
        {
            int size = Math.Min(width, height) / 2;

            // Left circle
            _graphics.FillEllipse(new SolidBrush(Color.Red), cx - size, cy - size / 2, size, size);
            _graphics.DrawEllipse(_pen, cx - size, cy - size / 2, size, size);

            // Right circle
            _graphics.FillEllipse(new SolidBrush(Color.Red), cx, cy - size / 2, size, size);
            _graphics.DrawEllipse(_pen, cx, cy - size / 2, size, size);

            // Bottom triangle
            Point[] triangle = new Point[]
            {
        new Point(cx - size, cy - size/4),
        new Point(cx + size, cy - size/4),
        new Point(cx, cy + size/2)
            };
            _graphics.FillPolygon(new SolidBrush(Color.Red), triangle);
            _graphics.DrawPolygon(_pen, triangle);
        }

        /// <summary>
        /// Handle unknown shape types via reflection
        /// This is the key method for Adapter pattern - it draws any shape
        /// from friend's plugin without knowing its type at compile time
        /// </summary>
        public void VisitUnknown(object shape)
        {
            if (shape == null) return;

            string typeName = shape.GetType().Name;

            // HEART SHAPE - Friend's plugin
            if (typeName == "Heart")
            {
                DrawHeartCorrectly(shape);
                return;
            }


            //if (shape == null) return;

            //string typeName = shape.GetType().Name;

            //// ============================================================
            //// HEART SHAPE - Friend's plugin support via reflection
            //// No direct reference to Heart class - everything via reflection
            //// ============================================================
            //if (typeName == "Heart")
            //{
            //    DrawHeartViaReflection(shape);
            //    return;
            //}

            // ============================================================
            // STAR SHAPE - Plugin support via reflection
            // ============================================================
            if (typeName == "Star")
            {
                DrawStarViaReflection(shape);
                return;
            }

            // ============================================================
            // PENTAGON SHAPE - Plugin support via reflection
            // ============================================================
            if (typeName == "Pentagon")
            {
                DrawPentagonViaReflection(shape);
                return;
            }

            // Generic drawing for any shape with GetPoints/GetVertices method
            var getPointsMethod = shape.GetType().GetMethod("GetPoints");
            var getVerticesMethod = shape.GetType().GetMethod("GetVertices");
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
                var radiusProp = shape.GetType().GetProperty("Radius");

                if (centerXProp != null && centerYProp != null)
                {
                    int cx = Convert.ToInt32(centerXProp.GetValue(shape));
                    int cy = Convert.ToInt32(centerYProp.GetValue(shape));

                    if (radiusProp != null)
                    {
                        int r = Convert.ToInt32(radiusProp.GetValue(shape));
                        _graphics.DrawEllipse(_pen, cx - r, cy - r, r * 2, r * 2);
                    }
                    else
                    {
                        _graphics.DrawLine(_pen, cx - 10, cy, cx + 10, cy);
                        _graphics.DrawLine(_pen, cx, cy - 10, cx, cy + 10);
                    }
                }
            }
        }

        // Добавьте этот метод в класс DynamicShapeVisitor
        private void DrawHeart(object heart)
        {
            try
            {
                // Get center coordinates
                int cx = 100, cy = 100;
                var centerXProp = heart.GetType().GetProperty("CenterX");
                var centerYProp = heart.GetType().GetProperty("CenterY");

                if (centerXProp != null && centerYProp != null)
                {
                    cx = Convert.ToInt32(centerXProp.GetValue(heart));
                    cy = Convert.ToInt32(centerYProp.GetValue(heart));
                }

                // Draw heart using simple shapes
                int size = 35;

                // Left circle
                _graphics.FillEllipse(new SolidBrush(Color.Red), cx - size, cy - size / 2, size, size);
                _graphics.DrawEllipse(_pen, cx - size, cy - size / 2, size, size);

                // Right circle
                _graphics.FillEllipse(new SolidBrush(Color.Red), cx, cy - size / 2, size, size);
                _graphics.DrawEllipse(_pen, cx, cy - size / 2, size, size);

                // Bottom triangle
                Point[] triangle = new Point[]
                {
            new Point(cx - size, cy - size/4),
            new Point(cx + size, cy - size/4),
            new Point(cx, cy + size/2)
                };
                _graphics.FillPolygon(new SolidBrush(Color.Red), triangle);
                _graphics.DrawPolygon(_pen, triangle);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"DrawHeart error: {ex.Message}");
            }
        }

        /// <summary>
        /// Draw Heart shape using reflection - NO direct reference to Heart class
        /// This is pure Adapter pattern working at runtime
        /// </summary>
        private void DrawHeartViaReflection(object heart)
        {
            try
            {
                // Get CenterX and CenterY via reflection
                var centerXProp = heart.GetType().GetProperty("CenterX");
                var centerYProp = heart.GetType().GetProperty("CenterY");
                var locationProp = heart.GetType().GetProperty("Location");

                int cx = 100, cy = 100;

                if (centerXProp != null && centerYProp != null)
                {
                    cx = Convert.ToInt32(centerXProp.GetValue(heart));
                    cy = Convert.ToInt32(centerYProp.GetValue(heart));
                }
                else if (locationProp != null)
                {
                    var location = locationProp.GetValue(heart);
                    var xProp = location?.GetType().GetProperty("X");
                    var yProp = location?.GetType().GetProperty("Y");
                    if (xProp != null) cx = Convert.ToInt32(xProp.GetValue(location));
                    if (yProp != null) cy = Convert.ToInt32(yProp.GetValue(location));
                }

                // Try to get heart points via GetHeartPoints method
                var getPointsMethod = heart.GetType().GetMethod("GetHeartPoints");
                if (getPointsMethod != null)
                {
                    var result = getPointsMethod.Invoke(heart, null);
                    if (result is PointF[] pointsF && pointsF.Length > 0)
                    {
                        Point[] points = pointsF.Select(p => new Point((int)p.X, (int)p.Y)).ToArray();
                        using (SolidBrush brush = new SolidBrush(Color.Red))
                        {
                            _graphics.FillPolygon(brush, points);
                        }
                        _graphics.DrawPolygon(_pen, points);
                        return;
                    }
                }

                // Fallback: Draw simple heart using ellipses and polygon
                DrawSimpleHeart(cx, cy);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"DrawHeart error: {ex.Message}");
                DrawSimpleHeart(100, 100);
            }
        }

        /// <summary>
        /// Fallback heart drawing method
        /// </summary>
        private void DrawSimpleHeart(int cx, int cy)
        {
            int size = 30;

            // Left semi-circle
            _graphics.DrawEllipse(_pen, cx - size, cy - size / 2, size, size);
            // Right semi-circle
            _graphics.DrawEllipse(_pen, cx, cy - size / 2, size, size);
            // Bottom triangle
            Point[] triangle = new Point[]
            {
                new Point(cx - size, cy - size/4),
                new Point(cx + size, cy - size/4),
                new Point(cx, cy + size/2)
            };
            _graphics.DrawPolygon(_pen, triangle);

            // Fill with red
            using (SolidBrush brush = new SolidBrush(Color.Red))
            {
                _graphics.FillEllipse(brush, cx - size, cy - size / 2, size, size);
                _graphics.FillEllipse(brush, cx, cy - size / 2, size, size);
                _graphics.FillPolygon(brush, triangle);
            }
        }

        /// <summary>
        /// Draw Star shape using reflection
        /// </summary>
        private void DrawStarViaReflection(object star)
        {
            try
            {
                var getPointsMethod = star.GetType().GetMethod("GetPoints");
                if (getPointsMethod != null)
                {
                    var result = getPointsMethod.Invoke(star, null);
                    if (result is Point[] points && points.Length > 0)
                    {
                        using (SolidBrush brush = new SolidBrush(Color.Gold))
                        {
                            _graphics.FillPolygon(brush, points);
                        }
                        _graphics.DrawPolygon(_pen, points);
                        return;
                    }
                }
            }
            catch { }
        }

        /// <summary>
        /// Draw Pentagon shape using reflection
        /// </summary>
        private void DrawPentagonViaReflection(object pentagon)
        {
            try
            {
                var getVerticesMethod = pentagon.GetType().GetMethod("GetVertices");
                if (getVerticesMethod != null)
                {
                    var result = getVerticesMethod.Invoke(pentagon, null);
                    if (result is Point[] points && points.Length > 0)
                    {
                        using (SolidBrush brush = new SolidBrush(Color.LightGreen))
                        {
                            _graphics.FillPolygon(brush, points);
                        }
                        _graphics.DrawPolygon(_pen, points);
                        return;
                    }
                }
            }
            catch { }
        }
    }
}