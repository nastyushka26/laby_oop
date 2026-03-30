namespace laba2oop
{
    // class for circle objects
    public class Circle: Shape
    {
        // radius for the circle
        public int Rad;

        public Circle(int Xcoord, int Ycoord, int Radius) : base(Xcoord, Ycoord)
        {
            this.Rad = Radius;
        }
        public override void Accept(IShapeVisitor visitor)
        {
            visitor.Visit(this);
        }
    }
}
