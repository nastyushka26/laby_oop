namespace laba1oop
{
    internal class Ellipse : Shape
    {
        public int a, b;
        public Ellipse(int x, int y, int aCoord, int bCoord) : base(x, y)
        {
            a = aCoord;
            b = bCoord;
        }
        public override void Draw(Graphics g, Pen pen, SolidBrush brush)
        {
            pen.Color = Color.Black;
            //pen.Color = Color.Lavender;
            brush.Color = Color.Lavender;
            //base.Draw(g, pen, brush);
            //Console.WriteLine($"Ellipse({a}, {b})");
            g.DrawEllipse(pen, CoordX, CoordY, a * 2, b * 2);
            g.FillEllipse(brush, CoordX, CoordY, a * 2, b * 2); 
        }
    }
}
