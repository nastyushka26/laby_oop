namespace laba1oop
{
    // class for line objects
    internal class Line : Shape
    {
        // additional fields - coordinates of second point
        public int x2, y2;
        public Line(int x1, int y1, int x2, int y2) : base(x1, y1)
        {
            this.x2 = x2;
            this.y2 = y2;
        }
        
    }
}
