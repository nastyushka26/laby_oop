using Microsoft.VisualBasic;
using PluginInterface;
using System.Text.Json;
using System.Text;

namespace laba2oop
{
    public partial class MainForm : Form
    {
        private ShapeList shapeList;
        private List<IShapeFactory> factories;
        private IShapeFactory currentFactory;
        private PluginManager _pluginManager;

        // Processors for encryption - DEFAULT EMPTY (NO ENCRYPTION)
        private List<IDataProcessorPlugin> _activeProcessors = new List<IDataProcessorPlugin>();
        private ToolStripMenuItem _settingsMenuItem;
        private ToolStripMenuItem _saveMenuItem;
        private ToolStripMenuItem _loadMenuItem;
        private ToolStripMenuItem _resetProcessorsMenuItem;

        public MainForm()
        {
            InitializeComponent();
            this.DoubleBuffered = true;

            shapeList = new ShapeList();
            factories = new List<IShapeFactory>();

            InitializeMenu();
            InitializePlugins();
            InitializeFactories();
            CreateButtonsFromFactories();

            UpdatePluginStatus();
        }

        private void InitializeMenu()
        {
            MenuStrip menuStrip = new MenuStrip();

            var fileMenu = new ToolStripMenuItem("File");
            _saveMenuItem = new ToolStripMenuItem("Save to File...", null, SaveToFile_Click);
            _loadMenuItem = new ToolStripMenuItem("Load from File...", null, LoadFromFile_Click);
            fileMenu.DropDownItems.Add(_saveMenuItem);
            fileMenu.DropDownItems.Add(_loadMenuItem);

            var toolsMenu = new ToolStripMenuItem("Tools");
            _settingsMenuItem = new ToolStripMenuItem("Data Processor Settings...", null, Settings_Click);
            toolsMenu.DropDownItems.Add(_settingsMenuItem);

            _resetProcessorsMenuItem = new ToolStripMenuItem("Reset Processors (Disable Encryption)", null, ResetProcessors_Click);
            toolsMenu.DropDownItems.Add(_resetProcessorsMenuItem);

            menuStrip.Items.Add(fileMenu);
            menuStrip.Items.Add(toolsMenu);

            this.MainMenuStrip = menuStrip;
            this.Controls.Add(menuStrip);

            menuStrip.Dock = DockStyle.Top;
            panel_buttons.Dock = DockStyle.Right;
            panelPaint.Dock = DockStyle.Fill;

            menuStrip.BringToFront();
        }

        private void ResetProcessors_Click(object sender, EventArgs e)
        {
            _activeProcessors.Clear();
            statusLabel.Text = "Encryption DISABLED. Files will be saved as plain JSON.";
            MessageBox.Show("All encryption processors have been removed.\n\nFiles will now be saved as plain JSON (no encryption).",
                "Encryption Disabled", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void UpdatePluginStatus()
        {
            int shapePluginsCount = _pluginManager.PluginFactories.Count;
            int processorPluginsCount = _pluginManager.DataProcessors.Count;
            int activeProcessorsCount = _activeProcessors.Count;

            if (activeProcessorsCount > 0)
            {
                statusLabel.Text = $"Encryption: ON ({activeProcessorsCount} processor(s)) | Shapes: {shapePluginsCount} | Available: {processorPluginsCount}";
            }
            else
            {
                statusLabel.Text = $"Encryption: OFF (plain JSON) | Shapes: {shapePluginsCount} | Available: {processorPluginsCount}";
            }
        }

        private void InitializePlugins()
        {
            _pluginManager = new PluginManager();

            string pluginsDir = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Plugins");

            if (!Directory.Exists(pluginsDir))
            {
                Directory.CreateDirectory(pluginsDir);
                string readmePath = Path.Combine(pluginsDir, "Readme.txt");
                File.WriteAllText(readmePath, "Place your plugin DLLs here.\n\nExample plugins:\n- StarPlugin.dll\n- PentagonPlugin.dll");
            }

            _pluginManager.LoadPluginsFromDirectory(pluginsDir, verifySignature: false);

            foreach (var pluginFactory in _pluginManager.PluginFactories)
            {
                var adapter = new PluginFactoryAdapter(pluginFactory);
                factories.Add(adapter);
            }
        }

        private void InitializeFactories()
        {
            factories.Add(new CircleFactory());
            factories.Add(new LineFactory());
            factories.Add(new SquareFactory());
            factories.Add(new TriangleFactory());
            factories.Add(new EllipseFactory());
            factories.Add(new RectangleFactory());
        }

        private void CreateButtonsFromFactories()
        {
            panel_buttons.Controls.Clear();

            int yPos = 10;
            int buttonHeight = 40;
            int buttonWidth = panel_buttons.Width - 20;

            if (buttonWidth <= 0) buttonWidth = 100;

            for (int i = 0; i < factories.Count; i++)
            {
                Button btn = new Button();

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
                btn.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;

                panel_buttons.Controls.Add(btn);
                yPos += buttonHeight + 5;
            }

            panel_buttons.AutoScroll = true;
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

        private void panelPaint_Paint(object sender, PaintEventArgs e)
        {
            if (shapeList.ArrayShapes.Count == 0) return;

            Graphics graph = e.Graphics;
            using (Pen pen = new Pen(Color.Black, 2))
            {
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

        private void Settings_Click(object sender, EventArgs e)
        {
            using (var settingsForm = new PluginSettingsForm(_pluginManager.DataProcessors.ToList(), _activeProcessors))
            {
                if (settingsForm.ShowDialog() == DialogResult.OK)
                {
                    _activeProcessors = settingsForm.SelectedProcessors;
                    UpdatePluginStatus();
                }
            }
        }

        private void SaveToFile_Click(object sender, EventArgs e)
        {
            if (shapeList.ArrayShapes.Count == 0)
            {
                MessageBox.Show("No shapes to save.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            using (SaveFileDialog sfd = new SaveFileDialog())
            {
                sfd.Filter = "JSON File (*.json)|*.json|Encrypted Shape Data (*.eshd)|*.eshd|All Files (*.*)|*.*";
                sfd.DefaultExt = "json";
                sfd.FileName = "shapes";

                if (sfd.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        string jsonData = SerializeShapes();
                        byte[] jsonBytes = Encoding.UTF8.GetBytes(jsonData);

                        byte[] dataToSave = jsonBytes;

                        if (_activeProcessors.Count > 0)
                        {
                            foreach (var processor in _activeProcessors)
                            {
                                dataToSave = processor.ProcessBeforeSave(dataToSave);
                            }
                            statusLabel.Text = $"Saved with encryption ({_activeProcessors.Count} processor(s))";
                        }
                        else
                        {
                            statusLabel.Text = $"Saved as plain JSON";
                        }

                        File.WriteAllBytes(sfd.FileName, dataToSave);

                        MessageBox.Show($"Saved successfully!\n\nFile: {Path.GetFileName(sfd.FileName)}\nEncryption: {(_activeProcessors.Count > 0 ? "ON" : "OFF")}\nShapes saved: {shapeList.ArrayShapes.Count}",
                            "Save Complete", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Save failed: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        private void LoadFromFile_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog ofd = new OpenFileDialog())
            {
                ofd.Filter = "JSON Files (*.json)|*.json|Encrypted Shape Data (*.eshd)|*.eshd|All Files (*.*)|*.*";

                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        byte[] fileData = File.ReadAllBytes(ofd.FileName);

                        bool isPlainJson = false;
                        string testString = Encoding.UTF8.GetString(fileData);
                        string trimmed = testString.TrimStart();
                        if (trimmed.StartsWith("{") || trimmed.StartsWith("["))
                        {
                            isPlainJson = true;
                        }

                        byte[] dataToLoad = fileData;

                        if (!isPlainJson && _activeProcessors.Count > 0)
                        {
                            for (int i = _activeProcessors.Count - 1; i >= 0; i--)
                            {
                                dataToLoad = _activeProcessors[i].ProcessAfterLoad(dataToLoad);
                            }
                        }
                        else if (!isPlainJson && _activeProcessors.Count == 0)
                        {
                            DialogResult result = MessageBox.Show(
                                "This file appears to be encrypted, but no encryption processors are active.\n\n" +
                                "Do you want to open Data Processor Settings to add processors?",
                                "Encrypted File Detected",
                                MessageBoxButtons.YesNo,
                                MessageBoxIcon.Question);

                            if (result == DialogResult.Yes)
                            {
                                Settings_Click(sender, e);
                                LoadFromFile_Click(sender, e);
                                return;
                            }
                        }

                        string jsonString = Encoding.UTF8.GetString(dataToLoad);
                        DeserializeShapes(jsonString);

                        panelPaint.Invalidate();
                        statusLabel.Text = $"Loaded from {Path.GetFileName(ofd.FileName)}";
                        MessageBox.Show($"Loaded successfully!\n\nShapes loaded: {shapeList.ArrayShapes.Count}",
                            "Load Complete", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Load failed: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        private string SerializeShapes()
        {
            var shapesDto = new List<ShapeDto>();
            foreach (var shape in shapeList.ArrayShapes)
            {
                shapesDto.Add(ShapeToDto(shape));
            }
            return JsonSerializer.Serialize(shapesDto, new JsonSerializerOptions { WriteIndented = true });
        }

        private void DeserializeShapes(string json)
        {
            json = json.Trim();

            if (string.IsNullOrEmpty(json))
            {
                throw new Exception("JSON data is empty");
            }

            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };

            var shapesDto = JsonSerializer.Deserialize<List<ShapeDto>>(json, options);
            if (shapesDto != null)
            {
                shapeList.ArrayShapes.Clear();
                shapeList.CountShapes = 0;

                foreach (var dto in shapesDto)
                {
                    var shape = DtoToShape(dto);
                    if (shape != null)
                    {
                        shapeList.Add(shape);
                    }
                }
            }
        }

        [Serializable]
        public class ShapeDto
        {
            public string Type { get; set; } = "";
            public int CoordX { get; set; }
            public int CoordY { get; set; }
            public int? Radius { get; set; }
            public int? Width { get; set; }
            public int? Height { get; set; }
            public int? Len { get; set; }
            public int? X2 { get; set; }
            public int? Y2 { get; set; }
            public int? X3 { get; set; }
            public int? Y3 { get; set; }
            public int? A { get; set; }
            public int? B { get; set; }
            public string? PluginShapeType { get; set; }
            public string? PluginData { get; set; }
        }

        private ShapeDto ShapeToDto(Shape shape)
        {
            var dto = new ShapeDto { CoordX = shape.CoordX, CoordY = shape.CoordY };

            switch (shape)
            {
                case Circle c:
                    dto.Type = "Circle";
                    dto.Radius = c.Rad;
                    break;
                case Rectangle r:
                    dto.Type = "Rectangle";
                    dto.Width = r.width;
                    dto.Height = r.height;
                    break;
                case Square s:
                    dto.Type = "Square";
                    dto.Len = s.len;
                    break;
                case Line l:
                    dto.Type = "Line";
                    dto.X2 = l.x2;
                    dto.Y2 = l.y2;
                    break;
                case Triangle t:
                    dto.Type = "Triangle";
                    dto.X2 = t.x2;
                    dto.Y2 = t.y2;
                    dto.X3 = t.x3;
                    dto.Y3 = t.y3;
                    break;
                case Ellipse e:
                    dto.Type = "Ellipse";
                    dto.A = e.a;
                    dto.B = e.b;
                    break;
                case PluginShapeWrapper wrapper:
                    dto.Type = "PluginShape";
                    dto.PluginShapeType = wrapper.ShapeType;
                    dto.PluginData = wrapper.Serialize();
                    break;
                default:
                    dto.Type = "Unknown";
                    break;
            }
            return dto;
        }

        private Shape DtoToShape(ShapeDto dto)
        {
            switch (dto.Type)
            {
                case "Circle":
                    if (dto.Radius.HasValue)
                        return new Circle(dto.CoordX, dto.CoordY, dto.Radius.Value);
                    break;
                case "Rectangle":
                    if (dto.Width.HasValue && dto.Height.HasValue)
                        return new Rectangle(dto.CoordX, dto.CoordY, dto.Width.Value, dto.Height.Value);
                    break;
                case "Square":
                    if (dto.Len.HasValue)
                        return new Square(dto.CoordX, dto.CoordY, dto.Len.Value);
                    break;
                case "Line":
                    if (dto.X2.HasValue && dto.Y2.HasValue)
                        return new Line(dto.CoordX, dto.CoordY, dto.X2.Value, dto.Y2.Value);
                    break;
                case "Triangle":
                    if (dto.X2.HasValue && dto.Y2.HasValue && dto.X3.HasValue && dto.Y3.HasValue)
                        return new Triangle(dto.CoordX, dto.CoordY, dto.X2.Value, dto.Y2.Value, dto.X3.Value, dto.Y3.Value);
                    break;
                case "Ellipse":
                    if (dto.A.HasValue && dto.B.HasValue)
                        return new Ellipse(dto.CoordX, dto.CoordY, dto.A.Value, dto.B.Value);
                    break;
                case "PluginShape":
                    if (!string.IsNullOrEmpty(dto.PluginShapeType) && !string.IsNullOrEmpty(dto.PluginData))
                    {
                        var factory = FindFactoryForShapeType(dto.PluginShapeType);
                        if (factory != null)
                        {
                            var pluginShape = factory.DeserializeShape(dto.PluginData, dto.CoordX, dto.CoordY);
                            if (pluginShape != null)
                            {
                                return new PluginShapeWrapper(pluginShape, dto.PluginShapeType, factory);
                            }
                        }
                    }
                    break;
            }
            return null;
        }

        /// <summary>
        /// Find the plugin factory that can create a given shape type
        /// </summary>
        private IShapeFactoryPlugin? FindFactoryForShapeType(string shapeType)
        {
            foreach (var factory in _pluginManager.PluginFactories)
            {
                if (factory.ShapeName == shapeType)
                {
                    return factory;
                }
            }
            return null;
        }

        private void panelPaint_Paint_1(object sender, PaintEventArgs e) { }
    }
}