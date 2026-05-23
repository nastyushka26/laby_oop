using PluginInterface;
using Microsoft.VisualBasic;

namespace laba2oop
{
    public partial class MainForm : Form
    {
        private ShapeList shapeList;
        private List<IShapeFactory> factories;
        private IShapeFactory currentFactory;
        private PluginManager _pluginManager;

        public MainForm()
        {
            InitializeComponent();
            this.DoubleBuffered = true;

            shapeList = new ShapeList();
            factories = new List<IShapeFactory>();

            // Initialize plugin system
            InitializePlugins();

            // Initialize built-in factories
            InitializeFactories();

            // Create buttons from all factories (built-in + plugins)
            CreateButtonsFromFactories();
        }

        /// <summary>
        /// Initialize plugin system and load plugins
        /// </summary>
        private void InitializePlugins()
        {
            _pluginManager = new PluginManager();

            // Load plugins from Plugins folder
            string pluginsDir = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Plugins");
            int loadedCount = _pluginManager.LoadPluginsFromDirectory(pluginsDir, verifySignature: true);

            // Also check command line argument
            string[] args = Environment.GetCommandLineArgs();
            for (int i = 1; i < args.Length; i++)
            {
                if (args[i].StartsWith("--plugin="))
                {
                    string pluginName = args[i].Substring(9);
                    _pluginManager.LoadPluginFromArgument(pluginName);
                }
            }

            // Add plugin factories to factories list
            foreach (var pluginFactory in _pluginManager.PluginFactories)
            {
                var adapter = _pluginManager.CreateFactoryAdapter(pluginFactory);
                factories.Add(adapter);
            }

            // Display loaded plugins info
            if (_pluginManager.LoadedPlugins.Count > 0)
            {
                statusLabel.Text = $"Loaded {_pluginManager.LoadedPlugins.Count} plugin(s)";
            }
        }

        private void InitializeFactories()
        {
            // Add all built-in factories
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
            int buttonHeight = 40;
            int buttonWidth = panel_buttons.Width - 20;

            for (int i = 0; i < factories.Count; i++)
            {
                Button btn = new Button();

                // Get shape name from factory
                if (factories[i] is ShapeBaseFactory baseFactory)
                {
                    btn.Text = baseFactory.ShapeName;
                }
                else if (factories[i] is PluginFactoryAdapter adapter)
                {
                    btn.Text = adapter.ShapeName;
                }
                else
                {
                    btn.Text = "Shape";
                }

                btn.Location = new Point(10, yPos);
                btn.Size = new Size(buttonWidth, buttonHeight);
                btn.Tag = factories[i];
                btn.Click += BtnShape_Click;
                btn.BackColor = factories[i] is PluginFactoryAdapter ? Color.LightYellow : Color.LightGray;

                panel_buttons.Controls.Add(btn);
                yPos += buttonHeight + 5;
            }
        }

        private void BtnShape_Click(object sender, EventArgs e)
        {
            Button clickedBtn = sender as Button;
            currentFactory = clickedBtn.Tag as IShapeFactory;
            currentFactory.Reset();

            string shapeName = "";
            if (currentFactory is ShapeBaseFactory baseFactory)
                shapeName = baseFactory.ShapeName;
            else if (currentFactory is PluginFactoryAdapter adapter)
                shapeName = adapter.ShapeName;

            statusLabel.Text = $"Drawing {shapeName}:\nMake clicks...";
        }

        // method for painting created figures
        private void panelPaint_Paint(object sender, PaintEventArgs e)
        {
            if (shapeList.ArrayShapes.Count == 0) return;

            Graphics graph = e.Graphics;
            using (Pen pen = new Pen(Color.Black, 2))
            {
                // ИСПОЛЬЗУЙТЕ DynamicShapeVisitor вместо ShapeDrawingVisitor
                var drawingVisitor = new DynamicShapeVisitor(graph, pen);

                foreach (var shape in shapeList.ArrayShapes)
                {
                    shape.Accept(drawingVisitor);
                }
            }
        }

        private void panel1_MouseClick(object sender, MouseEventArgs e)
        {
            if (currentFactory != null)
            {
                int x = e.X;
                int y = e.Y;

                currentFactory.GetCoordinates(x, y);

                if (currentFactory.IsReady())
                {
                    try
                    {
                        Shape newShape = currentFactory.CreateShape();
                        if (newShape != null)
                        {
                            shapeList.Add(newShape);
                            panelPaint.Invalidate();
                            statusLabel.Text = $"Shape created!\nSelect next shape";
                        }
                    }
                    catch (Exception ex)
                    {
                        statusLabel.Text = $"Creation error:\n{ex.Message}";
                    }
                    finally
                    {
                        currentFactory.Reset();
                        currentFactory = null;
                    }
                }
                else
                {
                    int clicksLeft = currentFactory.need_click_count - currentFactory.index_now;
                    statusLabel.Text = $"Need {clicksLeft} more click(s)";
                }
            }
            else
            {
                statusLabel.Text = "Please select a shape first";
            }
        }

        private void panelPaint_Paint_1(object sender, PaintEventArgs e)
        {

        }
    }
}