using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace laba2oop
{
    public partial class NewMainForm : Form
    {
        private ShapeList shapeList;
        private List<IShapeFactory> factories; // array of all factories
        private IShapeFactory currentFactory;
        public NewMainForm()
        {
            InitializeComponent();
            this.DoubleBuffered = true;
            //shapeList = new ShapeList();
            //shapeList.Add(new Rectangle(100, 100, 250, 170));
            //shapeList.Add(new Triangle(200, 80, 470, 380, 650, 400));
            //shapeList.Add(new Square(600, 80, 400));
            //shapeList.Add(new Circle(200, 250, 150));
            //shapeList.Add(new Ellipse(400, 200, 150, 250));
            //shapeList.Add(new Line(70, 80, 1200, 410));
            //shapeList.Add(new Rectangle(300, 400, 100, 200));
            //shapeList.Add(new Square(500, 400, 300));

            shapeList = new ShapeList();
            // creating and initializing factories array
            InitializeFactories();

            // creating buttons from factories list
            CreateButtonsFromFactories();


        }

        private void InitializeFactories()
        {
            factories = new List<IShapeFactory>();

            // adding all factories
            factories.Add(new CircleFactory());
            factories.Add(new LineFactory());
            factories.Add(new SquareFactory());
            factories.Add(new TriangleFactory());
            factories.Add(new EllipseFactory());
            factories.Add(new RectangleFactory());
        }

        private void CreateButtonsFromFactories()
        {
            int yPos = 10;
            int buttonHeight = 70;
            int buttonWidth = panel_buttons.Width;

            for (int i = 0; i < factories.Count; i++)
            {
                Button btn = new Button();
                btn.Text = (factories[i] as ShapeBaseFactory)?.ShapeName ?? "Фигура";
                btn.Location = new Point(0, yPos);
                btn.Size = new Size(buttonWidth, buttonHeight);
                btn.Tag = factories[i];  // saving reference to factory in button
                btn.Click += BtnShape_Click; // uploading currentFactory

                panel_buttons.Controls.Add(btn);
                yPos += buttonHeight + 5;
            }
        }

        private void BtnShape_Click(object sender, EventArgs e)
        {
            Button clickedBtn = sender as Button;
            currentFactory = clickedBtn.Tag as IShapeFactory;
            currentFactory.Reset(); // reset clicks before
            // обновляем статус
            ShapeBaseFactory baseFactory = currentFactory as ShapeBaseFactory;
            statusLabel.Text = $"Рисуем {baseFactory?.ShapeName}: \nсделайте клики...";
        }

        private void panelPaint_Paint(object sender, PaintEventArgs e)
        {
            Graphics graph = e.Graphics;
            Pen pen = new Pen(Color.Black, 2);

            // рисуем все фигуры из списка
            foreach (var shape in shapeList.ArrayShapes)
            {
                if (shape is Circle circle)
                {
                    graph.DrawEllipse(pen, circle.CoordX - circle.Rad,
                                            circle.CoordY - circle.Rad,
                                            circle.Rad * 2, circle.Rad * 2);
                }
                else if (shape is Rectangle rect)
                {
                    graph.DrawRectangle(pen, rect.CoordX, rect.CoordY,
                                              rect.width, rect.height);
                }
                else if (shape is Square square)
                {
                    graph.DrawRectangle(pen, square.CoordX, square.CoordY,
                                              square.len, square.len);
                }
                else if (shape is Line line)
                {
                    graph.DrawLine(pen, line.CoordX, line.CoordY,
                                           line.x2, line.y2);
                }
                else if (shape is Triangle triangle)
                {
                    Point[] points = new Point[]
                    {
                        new Point(triangle.CoordX, triangle.CoordY),
                        new Point(triangle.x2, triangle.y2),
                        new Point(triangle.x3, triangle.y3)
                    };
                    graph.DrawPolygon(pen, points);
                }
                else if (shape is Ellipse ellipse)
                {
                    // coordX and CoordY - coordinates of center
                    graph.DrawEllipse(pen, ellipse.CoordX - ellipse.a,
                                            ellipse.CoordY - ellipse.b,
                                            ellipse.a * 2, ellipse.b * 2);
                }
            }

            pen.Dispose();
        }

        private void panelPaint_MouseClick(object sender, MouseEventArgs e)
        {
            if (currentFactory != null) // user chose figure to create
            {
                int x = e.X;
                int y = e.Y;

                // passing mause coordinates to factory
                currentFactory.GetCoordinates(x, y);

                // checking if we can receive new object of current Shape
                if (currentFactory.IsReady())
                {
                    try
                    {
                        // receiving new shape from constructor
                        Shape newShape = currentFactory.CreateShape();
                        if (newShape != null)
                        {
                            // adding it to the list
                            shapeList.Add(newShape);
                            panelPaint.Invalidate();
                            statusLabel.Text = $"Фигура создана! \nВыберите следующую";
                        }
                    }
                    catch (Exception ex)
                    {
                        statusLabel.Text = $"Ошибка создания: \n{ex.Message}";
                    }
                    finally
                    {
                        currentFactory.Reset(); // reseting for next figure
                        currentFactory = null;
                    }
                }
                else
                {
                    ShapeBaseFactory baseFactory = currentFactory as ShapeBaseFactory;
                    int clicksLeft = (baseFactory?.need_click_count ?? 2) - (baseFactory?.index_now ?? 1);
                    statusLabel.Text = $"Нужно еще {clicksLeft} кликов";

                }
            }
            else
            {
                statusLabel.Text = "Сначала выберите фигуру";
                return;
            }
        
        }
    }
}
