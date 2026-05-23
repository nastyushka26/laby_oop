using System;
using System.Drawing;

namespace StarPlugin
{
    public class Star
    {
        public int CenterX { get; set; }
        public int CenterY { get; set; }
        public int OuterRadius { get; set; }
        public int InnerRadius { get; set; }
        public int Points { get; set; }

        public Star(int centerX, int centerY, int outerRadius, int innerRadius, int points = 5)
        {
            CenterX = centerX;
            CenterY = centerY;
            OuterRadius = outerRadius;
            InnerRadius = innerRadius;
            Points = points;
        }

        public Point[] GetPoints()
        {
            Point[] points = new Point[Points * 2];
            double angleStep = Math.PI * 2 / (Points * 2);
            double angle = -Math.PI / 2;

            for (int i = 0; i < Points * 2; i++)
            {
                double radius = (i % 2 == 0) ? OuterRadius : InnerRadius;
                int x = CenterX + (int)(radius * Math.Cos(angle));
                int y = CenterY + (int)(radius * Math.Sin(angle));
                points[i] = new Point(x, y);
                angle += angleStep;
            }
            return points;
        }
    }
}