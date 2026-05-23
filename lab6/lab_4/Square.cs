namespace laba2oop
{
    // class for square objects
    public class Square : Shape
    {
        // additiaonal field - side length
        public int len;
        public Square(int x, int y, int len) : base(x, y)
        {
            this.len = len;
        }
        public override void Accept(IShapeVisitor visitor)
        {
            visitor.Visit(this);
        }
    }
}
