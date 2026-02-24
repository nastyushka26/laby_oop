namespace laba1oop
{
    internal class Triangle : Shape
    {
        public int x1, y1, x2, y2, x3, y3;
        public Triangle(int x_1, int y_1, int x_2, int y_2, int x_3, int y_3) : base(x_1, y_1) { 
            x1 = x_1;
            y1 = y_1;
            x2 = x_2;
            y2 = y_2;
            x3 = x_3;
            y3 = y_3;
        }
        public override void Draw(Graphics g, Pen pen, SolidBrush brush)
        {
            Point[] points = new Point[3] { new Point(x1, y1), new Point(x2, y2), new Point(x3, y3) };
            //pen.Color = Color.Blue;
            pen.Color = Color.Black;
            brush.Color = Color.Blue;
            base.Draw(g, pen, brush);
            //Console.WriteLine($"Triangle({CoordX}, {CoordY}, {CoordZ})");
            //g.DrawLines(pen, x1, y1, x2, y2, x3, y3);
            g.DrawPolygon(pen, points);
            g.FillPolygon(brush, points);
        }
    }
}
