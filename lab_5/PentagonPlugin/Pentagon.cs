using System;
using System.Drawing;

namespace PentagonPlugin
{
    public class Pentagon
    {
        public int CenterX { get; set; }
        public int CenterY { get; set; }
        public int Radius { get; set; }

        public Pentagon(int centerX, int centerY, int radius)
        {
            CenterX = centerX;
            CenterY = centerY;
            Radius = radius;
        }

        public Point[] GetVertices()
        {
            Point[] vertices = new Point[5];
            double angleStep = 2 * Math.PI / 5;
            double angle = -Math.PI / 2;

            for (int i = 0; i < 5; i++)
            {
                int x = CenterX + (int)(Radius * Math.Cos(angle));
                int y = CenterY + (int)(Radius * Math.Sin(angle));
                vertices[i] = new Point(x, y);
                angle += angleStep;
            }
            return vertices;
        }
    }
}