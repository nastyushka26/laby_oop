namespace laba1oop
{
    // class for triangle objects
    internal class Triangle : Shape
    {
        // additiaonal fields - coordinates of another 2 points
        public int x2, y2, x3, y3;
        public Triangle(int x_1, int y_1, int x_2, int y_2, int x_3, int y_3) : base(x_1, y_1) { 
            x2 = x_2;
            y2 = y_2;
            x3 = x_3;
            y3 = y_3;
        }
        
    }
}
