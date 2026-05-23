using System;
using System.Text.Json;
using PluginInterface;
using Microsoft.VisualBasic;

namespace StarPlugin
{
    public class StarFactory : IShapeFactoryPlugin
    {
        private int _clickX, _clickY;
        private bool _hasClick = false;

        public string ShapeName => "Star";
        public int NeededClicks => 1;

        

        public void AddClick(int x, int y)
        {
            if (!_hasClick)
            {
                _clickX = x;
                _clickY = y;
                _hasClick = true;
            }
        }

        public bool IsReady() => _hasClick;

        public object CreateShape()
        {
            string outerStr = Interaction.InputBox("Enter outer radius:", "Create Star", "50");
            string innerStr = Interaction.InputBox("Enter inner radius:", "Create Star", "25");
            string pointsStr = Interaction.InputBox("Enter number of points (5-12):", "Create Star", "5");

            int outerRadius = 50, innerRadius = 25, points = 5;
            int.TryParse(outerStr, out outerRadius);
            int.TryParse(innerStr, out innerRadius);
            int.TryParse(pointsStr, out points);
            points = Math.Clamp(points, 5, 12);

            return new Star(_clickX, _clickY, outerRadius, innerRadius, points);
        }

        public void Reset() => _hasClick = false;

        public (int X, int Y)[] GetCoordinates()
        {
            return _hasClick ? new[] { (_clickX, _clickY) } : Array.Empty<(int, int)>();
        }

        /// <summary>
        /// Serialize Star shape to JSON string
        /// </summary>
        public string SerializeShape(object shape)
        {
            if (shape is Star star)
            {
                var data = new
                {
                    OuterRadius = star.OuterRadius,
                    InnerRadius = star.InnerRadius,
                    Points = star.Points
                };
                return JsonSerializer.Serialize(data);
            }
            return "{}";
        }

        /// <summary>
        /// Deserialize Star shape from JSON string
        /// </summary>
        public object DeserializeShape(string data, int defaultX, int defaultY)
        {
            try
            {
                var dto = JsonSerializer.Deserialize<StarDto>(data);
                return new Star(defaultX, defaultY, dto?.OuterRadius ?? 50, dto?.InnerRadius ?? 25, dto?.Points ?? 5);
            }
            catch
            {
                return new Star(defaultX, defaultY, 50, 25, 5);
            }
        }

        private class StarDto
        {
            public int OuterRadius { get; set; }
            public int InnerRadius { get; set; }
            public int Points { get; set; }
        }
    }

    
}