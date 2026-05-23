namespace laba2oop
{
    // class for rectangle objects
    public class Rectangle : Shape
    {
        public int width, height;
        public Rectangle(int x, int y, int width, int height) : base(x, y)
        {
            this.width = width;
            this.height = height;
        }
        public override void Accept(IShapeVisitor visitor)
        {
            visitor.Visit(this);
        }
    }
}
