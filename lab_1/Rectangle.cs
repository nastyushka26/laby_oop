namespace laba1oop
{
    internal class Rectangle : Shape
    {
        public int coordx, coordy, width, height;
        public Rectangle(int x, int y, int width, int height) : base(x, y)
        {
            coordx = x;
            coordy = y;
            this.width = width;
            this.height = height;
        }
        public override void Draw(Graphics g, Pen pen, SolidBrush brush)
        {
            //pen.Color = Color.Green;
            pen.Color = Color.Black;
            brush.Color = Color.Green;
            //base.Draw(g, pen, brush);
            //Console.WriteLine($"Rectangle({x1}, {y1}, {x2}, {y2})");
            g.DrawRectangle(pen, coordx, coordy, width, height);
            g.FillRectangle(brush, coordx, coordy, width, height);    
        }
    }
}
