namespace laba2oop
{
    public class ShapeList
    {
        //public Shape[] ArrayShapes;
        public List<Shape> ArrayShapes;
        public int CountShapes;
        public ShapeList() { 
            ArrayShapes = new List<Shape>();
        }
        public void Add(Shape shape)
        {
            ArrayShapes.Add(shape);
            CountShapes++;
        }
    }
}
