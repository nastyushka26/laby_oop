namespace laba2oop
{
    // class for circle objects
    internal class Circle: Shape
    {
        // radius for the circle
        public int Rad;

        public Circle(int Xcoord, int Ycoord, int Radius) : base(Xcoord, Ycoord)
        {
            this.Rad = Radius;
        }
    }
}
