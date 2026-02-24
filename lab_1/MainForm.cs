namespace laba1oop
{
    public partial class MainForm : Form
    {
        private ShapeList shapeList;
        public MainForm()
        {
            InitializeComponent();
            this.DoubleBuffered = true;
            shapeList = new ShapeList();
            shapeList.Add(new Rectangle(100, 100, 250, 170));
            shapeList.Add(new Triangle(200, 80, 470, 380, 650, 400));
            shapeList.Add(new Square(600, 80, 400));
            shapeList.Add(new Circle(200, 250, 150));
            shapeList.Add(new Ellipse(400, 200, 150, 250));
            shapeList.Add(new Line(70, 80, 1200, 410));
            shapeList.Add(new Rectangle(300, 400, 100, 200));
            shapeList.Add(new Square(500, 400, 300));
        }

        private void MainForm_Paint(object sender, PaintEventArgs e)
        {
            Graphics graph = e.Graphics;
            Pen pen = new Pen(Color.Black, 5);
            SolidBrush brush = new SolidBrush(Color.White);
            for (int i = 0; i < shapeList.CountShapes; i++)
            {
                shapeList.ArrayShapes[i].Draw(graph, pen, brush);
            }
        }
    }
}
