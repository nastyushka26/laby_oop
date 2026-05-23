namespace laba2oop
{
    // class for ellipse objects
    public class Ellipse : Shape
    {
        // semi-minor axis and semi-major axis 
        public int a, b;
        // constructor for making ellipse object
        public Ellipse(int x, int y, int aCoord, int bCoord) : base(x, y)
        {
            a = aCoord;
            b = bCoord;
        }
        public override void Accept(IShapeVisitor visitor)
        {
            visitor.Visit(this);
        }
    }
}
