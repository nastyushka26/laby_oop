namespace laba1oop
{
    // class for rectangle objects
    internal class Rectangle : Shape
    {
        public int width, height;
        public Rectangle(int x, int y, int width, int height) : base(x, y)
        {
            this.width = width;
            this.height = height;
        }
        
    }
}
