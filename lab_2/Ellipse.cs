namespace laba2oop
{
    // class for ellipse objects
    internal class Ellipse : Shape
    {
        // semi-minor axis and semi-major axis 
        public int a, b;
        public Ellipse(int x, int y, int aCoord, int bCoord) : base(x, y)
        {
            a = aCoord;
            b = bCoord;
        }
        
    }
}
