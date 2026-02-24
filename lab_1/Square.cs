namespace laba1oop
{
    internal class Square : Shape
    {
        public int coordx, coordy, len;
        public Square(int x, int y, int len) : base(x, y)
        {
            coordx = x;
            coordy = y;
            this.len = len;
        }
        public override void Draw(Graphics g, Pen pen, SolidBrush brush)
        {
            //base.Draw(g, pen, brush);
            //pen.Color = Color.Pink;
            pen.Color = Color.Black;
            brush.Color = Color.Pink;
            //Console.WriteLine($"Square({coordx}, {coordy}, {len})");
            g.DrawRectangle(pen, coordx, coordy, len, len);
            g.FillRectangle(brush, coordx, coordy, len, len);
        }
    }
}
