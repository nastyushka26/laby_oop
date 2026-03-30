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
        private ShapeList shapeList; // list that will contain figures
        private List<IShapeFactory> factories; // array of all factories
        private IShapeFactory currentFactory;
        public NewMainForm()
        {
            InitializeComponent();
            this.DoubleBuffered = true;
            // creating list of shapes
            shapeList = new ShapeList();
            // creating and initializing factories array
            InitializeFactories();

            // creating buttons from factories list
            CreateButtonsFromFactories();
        }

        // method for list of factories initializing 
        private void InitializeFactories()
        {
            factories = new List<IShapeFactory>();

            // adding all factories to the array 
            factories.Add(new CircleFactory());
            factories.Add(new LineFactory());
            factories.Add(new SquareFactory());
            factories.Add(new TriangleFactory());
            factories.Add(new EllipseFactory());
            factories.Add(new RectangleFactory());
        }

        // method for making buttons dynamically for figure creating
        private void CreateButtonsFromFactories()
        {
            // additional variables for creating buttons
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

        // method to start create figure - button click handler
        private void BtnShape_Click(object sender, EventArgs e)
        {
            Button clickedBtn = sender as Button;
            currentFactory = clickedBtn.Tag as IShapeFactory;
            currentFactory.Reset(); // reset clicks before

            // updating status
            ShapeBaseFactory baseFactory = currentFactory as ShapeBaseFactory;
            statusLabel.Text = $"Рисуем {baseFactory?.ShapeName}: \nсделайте клики...";
        }

        // method for painting created figures
        private void panelPaint_Paint(object sender, PaintEventArgs e)
        {
            if (shapeList.ArrayShapes.Count == 0) return;

            Graphics graph = e.Graphics;
            using (Pen pen = new Pen(Color.Black, 2))
            {
                // Создаем посетителя один раз
                var drawingVisitor = new ShapeDrawingVisitor(graph, pen);

                // Каждая фигура сама вызывает нужный метод Visit
                foreach (var shape in shapeList.ArrayShapes)
                {
                    shape.Accept(drawingVisitor);
                }
            }
        }

        // method to handle mouse clicks to pass coordinates to creating figure methods
        private void panelPaint_MouseClick(object sender, MouseEventArgs e)
        {
            if (currentFactory != null) // user chose figure to create
            {
                int x = e.X;
                int y = e.Y;

                // passing mouse coordinates to factory
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
                    // if exception accures
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
                    // updating user hints
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
