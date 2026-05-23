using System;
using PluginInterface;
using Microsoft.VisualBasic;

namespace PentagonPlugin
{
    public class PentagonFactory : IShapeFactoryPlugin
    {
        private int _clickX, _clickY;
        private bool _hasClick = false;

        public string ShapeName => "Pentagon";
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
            string radiusStr = Interaction.InputBox("Enter radius:", "Create Pentagon", "60");
            int radius = 60;
            int.TryParse(radiusStr, out radius);
            return new Pentagon(_clickX, _clickY, radius);
        }

        public void Reset() => _hasClick = false;
        public (int X, int Y)[] GetCoordinates() => _hasClick ? new[] { (_clickX, _clickY) } : Array.Empty<(int, int)>();
    }
}