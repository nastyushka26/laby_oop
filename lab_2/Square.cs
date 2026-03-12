namespace laba1oop
{
    internal class Square : Shape
    {
        public int len;
        public Square(int x, int y, int len) : base(x, y)
        {
            this.len = len;
        }
        public override void Draw(Graphics g, Pen pen, SolidBrush brush)
        {
            //base.Draw(g, pen, brush);
            //pen.Color = Color.Pink;
            pen.Color = Color.Black;
            brush.Color = Color.Pink;
            //Console.WriteLine($"Square({coordx}, {coordy}, {len})");
            g.DrawRectangle(pen, CoordX, CoordY, len, len);
            g.FillRectangle(brush, CoordX, CoordY, len, len);
        }
    }
}
