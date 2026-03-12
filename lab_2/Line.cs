namespace laba1oop
{
    internal class Line : Shape
    {
        public int x2, y2;
        public Line(int x1, int y1, int x2, int y2) : base(x1, y1)
        {
            this.x2 = x2;
            this.y2 = y2;
        }
        public override void Draw(Graphics g, Pen pen, SolidBrush brush)
        {
            pen.Color = Color.Yellow;
            //base.Draw(g, pen, brush);
            //Console.WriteLine($"Line({len})");
            g.DrawLine(pen, CoordX, CoordY, x2, y2);
        }
    }
}
