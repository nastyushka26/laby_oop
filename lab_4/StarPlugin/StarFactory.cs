using System;
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
        public (int X, int Y)[] GetCoordinates() => _hasClick ? new[] { (_clickX, _clickY) } : Array.Empty<(int, int)>();
    }
}