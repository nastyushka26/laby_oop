namespace laba1oop
{
    // class for square objects
    internal class Square : Shape
    {
        // additiaonal field - side length
        public int len;
        public Square(int x, int y, int len) : base(x, y)
        {
            this.len = len;
        }
        
    }
}
