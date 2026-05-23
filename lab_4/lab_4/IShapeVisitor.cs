namespace laba2oop
{
    public interface IShapeVisitor
    {
        void Visit(Circle circle);
        void Visit(Rectangle rectangle);
        void Visit(Square square);
        void Visit(Line line);
        void Visit(Triangle triangle);
        void Visit(Ellipse ellipse);
    }
}
