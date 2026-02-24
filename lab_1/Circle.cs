namespace laba1oop
{
    internal class Circle: Shape
    {

        public int X, Y; 
        public int Rad;

        public Circle(int Xcoord, int Ycoord, int Radius) : base(Xcoord, Ycoord)
        {
            this.X = Xcoord;
            this.Y = Ycoord;
            this.Rad = Radius;
        }
        public override void Draw(Graphics g, Pen pen, SolidBrush brush)
        {
            //pen.Color = Color.Navy;
            pen.Color = Color.Black;
            brush.Color = Color.Navy;
            //base.Draw(g, pen, brush);
            //Console.WriteLine($"Circle(Center {Center}, Radius {Radius})");
            g.DrawEllipse(pen, X, Y, Rad * 2, Rad * 2);
            g.FillEllipse(brush, X, Y, Rad * 2, Rad * 2);
        }
    }
}
