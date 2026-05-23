// MainForm.cs - Full implementation with Command and Builder patterns
// Lab 6: Adapter pattern for friend's HeartPlugin, Command pattern for Undo/Redo, Builder pattern for complex shapes
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

        // Command Pattern for Undo/Redo
        private CommandManager _commandManager;

        // Builder Pattern
        private ComplexShapeBuilder _shapeBuilder;
        private ShapeBuilderDirector _builderDirector;

        // Processors for encryption
        private List<IDataProcessorPlugin> _activeProcessors = new List<IDataProcessorPlugin>();
        private ToolStripMenuItem _settingsMenuItem;
        private ToolStripMenuItem _saveMenuItem;
        private ToolStripMenuItem _loadMenuItem;
        private ToolStripMenuItem _resetProcessorsMenuItem;

        // Undo/Redo menu items
        private ToolStripMenuItem _undoMenuItem;
        private ToolStripMenuItem _redoMenuItem;

        // Builder menu items
        private ToolStripMenuItem _buildHouseMenuItem;
        private ToolStripMenuItem _buildSmileyMenuItem;
        private ToolStripMenuItem _buildFlowerMenuItem;

        public MainForm()
        {
            InitializeComponent();
            this.DoubleBuffered = true;

            shapeList = new ShapeList();
            factories = new List<IShapeFactory>();

            // Initialize Command Manager (Command Pattern)
            _commandManager = new CommandManager();
            _commandManager.CommandExecuted += OnCommandExecuted;
            _commandManager.UndoRedoStateChanged += OnUndoRedoStateChanged;

            // Initialize Builder (Builder Pattern)
            _shapeBuilder = new ComplexShapeBuilder();
            _builderDirector = new ShapeBuilderDirector(_shapeBuilder);

            InitializeMenu();
            InitializePlugins();
            InitializeFactories();

            // LOAD ALL PLUGINS MANUALLY
            LoadAllPluginsManually();  

            CreateButtonsFromFactories();

            UpdatePluginStatus();
            UpdateUndoRedoMenu();
        }

        // Add this method to MainForm.cs
        private void LoadAllPluginsManually()
        {
            string pluginsDir = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Plugins");

            if (!Directory.Exists(pluginsDir))
            {
                Directory.CreateDirectory(pluginsDir);
                return;
            }

            // Load StarPlugin
            string starPath = Path.Combine(pluginsDir, "StarPlugin.dll");
            if (File.Exists(starPath))
            {
                try
                {
                    var assembly = System.Reflection.Assembly.LoadFrom(starPath);
                    var pluginType = assembly.GetType("StarPlugin.StarPluginEntry");
                    if (pluginType != null)
                    {
                        var plugin = Activator.CreateInstance(pluginType) as IShapePlugin;
                        if (plugin != null && plugin.Initialize())
                        {
                            var factory = plugin.GetShapeFactory();
                            if (factory != null)
                            {
                                var adapter = new PluginFactoryAdapter(factory);
                                factories.Add(adapter);
                                statusLabel.Text = "StarPlugin loaded!";
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    statusLabel.Text = $"StarPlugin error: {ex.Message}";
                }
            }

            // Load PentagonPlugin
            string pentagonPath = Path.Combine(pluginsDir, "PentagonPlugin.dll");
            if (File.Exists(pentagonPath))
            {
                try
                {
                    var assembly = System.Reflection.Assembly.LoadFrom(pentagonPath);
                    var pluginType = assembly.GetType("PentagonPlugin.PentagonPluginEntry");
                    if (pluginType != null)
                    {
                        var plugin = Activator.CreateInstance(pluginType) as IShapePlugin;
                        if (plugin != null && plugin.Initialize())
                        {
                            var factory = plugin.GetShapeFactory();
                            if (factory != null)
                            {
                                var adapter = new PluginFactoryAdapter(factory);
                                factories.Add(adapter);
                                statusLabel.Text = "PentagonPlugin loaded!";
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    statusLabel.Text = $"PentagonPlugin error: {ex.Message}";
                }
            }

            // Load HeartPlugin (friend's plugin)
            string heartPath = Path.Combine(pluginsDir, "HeartPlugin.dll");
            if (File.Exists(heartPath))
            {
                try
                {
                    var heartAdapter = new HeartPluginEntryAdapter();
                    if (heartAdapter.Initialize())
                    {
                        var factory = heartAdapter.GetShapeFactory();
                        if (factory != null)
                        {
                            var wrapperFactory = new PluginFactoryAdapterForHeart(factory);
                            factories.Add(wrapperFactory);
                            statusLabel.Text = "HeartPlugin (friend) loaded!";
                        }
                    }
                }
                catch (Exception ex)
                {
                    statusLabel.Text = $"HeartPlugin error: {ex.Message}";
                }
            }
        }

        private void OnCommandExecuted(object? sender, string description)
        {
            statusLabel.Text = description;
            panelPaint.Invalidate();
        }

        private void OnUndoRedoStateChanged(object? sender, EventArgs e)
        {
            UpdateUndoRedoMenu();
        }

        private void UpdateUndoRedoMenu()
        {
            if (_undoMenuItem != null)
            {
                _undoMenuItem.Enabled = _commandManager.CanUndo;
                _undoMenuItem.Text = _commandManager.CanUndo ? "Undo" : "Undo (disabled)";
            }
            if (_redoMenuItem != null)
            {
                _redoMenuItem.Enabled = _commandManager.CanRedo;
                _redoMenuItem.Text = _commandManager.CanRedo ? "Redo" : "Redo (disabled)";
            }
        }

        private void Undo_Click(object? sender, EventArgs e)
        {
            _commandManager.Undo();
            panelPaint.Invalidate();
        }

        private void Redo_Click(object? sender, EventArgs e)
        {
            _commandManager.Redo();
            panelPaint.Invalidate();
        }

        private void BuildHouse_Click(object? sender, EventArgs e)
        {
            // Use Builder Pattern to create a complex shape
            var complexShape = _builderDirector.BuildHouse(300, 200);
            var addCommand = new AddComplexShapeCommand(shapeList, complexShape);
            _commandManager.ExecuteCommand(addCommand);
            panelPaint.Invalidate();
            statusLabel.Text = "Built a house using Builder pattern!";
        }

        private void BuildSmiley_Click(object? sender, EventArgs e)
        {
            var complexShape = _builderDirector.BuildSmileyFace(300, 200);
            var addCommand = new AddComplexShapeCommand(shapeList, complexShape);
            _commandManager.ExecuteCommand(addCommand);
            panelPaint.Invalidate();
            statusLabel.Text = "Built a smiley face using Builder pattern!";
        }

        private void BuildFlower_Click(object? sender, EventArgs e)
        {
            var complexShape = _builderDirector.BuildFlower(300, 200);
            var addCommand = new AddComplexShapeCommand(shapeList, complexShape);
            _commandManager.ExecuteCommand(addCommand);
            panelPaint.Invalidate();
            statusLabel.Text = "Built a flower using Builder pattern!";
        }

        private void ClearAll_Click(object? sender, EventArgs e)
        {
            if (shapeList.ArrayShapes.Count > 0)
            {
                var clearCommand = new ClearAllCommand(shapeList);
                _commandManager.ExecuteCommand(clearCommand);
                panelPaint.Invalidate();
            }
        }

        private void AddFriendPlugin()
        {
            try
            {
                // Try to load HeartPlugin from Plugins folder
                string heartPluginPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory,
                    "Plugins", "HeartPlugin.dll");

                if (File.Exists(heartPluginPath))
                {
                    var assembly = System.Reflection.Assembly.LoadFrom(heartPluginPath);
                    var factoryType = assembly.GetType("HeartPlugin.HeartFactory");

                    if (factoryType != null)
                    {
                        var factoryInstance = Activator.CreateInstance(factoryType);
                        var adapter = new HeartPluginAdapter(factoryInstance);

                        // Create a wrapper factory for the adapter
                        var factoryWrapper = new PluginFactoryAdapterForHeart(adapter);
                        factories.Add(factoryWrapper);

                        statusLabel.Text = "Friend's HeartPlugin loaded successfully!";
                    }
                }
                else
                {
                    statusLabel.Text = "HeartPlugin.dll not found in Plugins folder";
                }
            }
            catch (Exception ex)
            {
                statusLabel.Text = $"Failed to load friend's plugin: {ex.Message}";
            }
        }

        private void InitializeMenu()
        {
            MenuStrip menuStrip = new MenuStrip();

            var fileMenu = new ToolStripMenuItem("File");
            _saveMenuItem = new ToolStripMenuItem("Save to File...", null, SaveToFile_Click);
            _loadMenuItem = new ToolStripMenuItem("Load from File...", null, LoadFromFile_Click);
            fileMenu.DropDownItems.Add(_saveMenuItem);
            fileMenu.DropDownItems.Add(_loadMenuItem);

            // Edit menu for Undo/Redo (Command Pattern)
            var editMenu = new ToolStripMenuItem("Edit");
            _undoMenuItem = new ToolStripMenuItem("Undo", null, Undo_Click);
            _redoMenuItem = new ToolStripMenuItem("Redo", null, Redo_Click);
            var clearAllMenuItem = new ToolStripMenuItem("Clear All", null, ClearAll_Click);
            editMenu.DropDownItems.Add(_undoMenuItem);
            editMenu.DropDownItems.Add(_redoMenuItem);
            editMenu.DropDownItems.Add(new ToolStripSeparator());
            editMenu.DropDownItems.Add(clearAllMenuItem);

            // Build menu for Builder Pattern
            var buildMenu = new ToolStripMenuItem("Build Complex");
            _buildHouseMenuItem = new ToolStripMenuItem("Build House", null, BuildHouse_Click);
            _buildSmileyMenuItem = new ToolStripMenuItem("Build Smiley Face", null, BuildSmiley_Click);
            _buildFlowerMenuItem = new ToolStripMenuItem("Build Flower", null, BuildFlower_Click);
            buildMenu.DropDownItems.Add(_buildHouseMenuItem);
            buildMenu.DropDownItems.Add(_buildSmileyMenuItem);
            buildMenu.DropDownItems.Add(_buildFlowerMenuItem);

            var toolsMenu = new ToolStripMenuItem("Tools");
            _settingsMenuItem = new ToolStripMenuItem("Data Processor Settings...", null, Settings_Click);
            toolsMenu.DropDownItems.Add(_settingsMenuItem);

            _resetProcessorsMenuItem = new ToolStripMenuItem("Reset Processors (Disable Encryption)", null, ResetProcessors_Click);
            toolsMenu.DropDownItems.Add(_resetProcessorsMenuItem);

            menuStrip.Items.Add(fileMenu);
            menuStrip.Items.Add(editMenu);
            menuStrip.Items.Add(buildMenu);
            menuStrip.Items.Add(toolsMenu);

            this.MainMenuStrip = menuStrip;
            this.Controls.Add(menuStrip);

            menuStrip.Dock = DockStyle.Top;
            panel_buttons.Dock = DockStyle.Right;
            panelPaint.Dock = DockStyle.Fill;

            menuStrip.BringToFront();
        }

        private void ResetProcessors_Click(object? sender, EventArgs e)
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
            }

            _pluginManager.LoadPluginsFromDirectory(pluginsDir, verifySignature: false);
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
                else if (factories[i] is PluginFactoryAdapterForHeart heartAdapter)
                {
                    btn.Text = heartAdapter.ShapeName;
                }
                else
                {
                    btn.Text = "Shape";
                }

                btn.Location = new Point(10, yPos);
                btn.Size = new Size(buttonWidth, buttonHeight);
                btn.Tag = factories[i];
                btn.Click += BtnShape_Click;
                btn.BackColor = (factories[i] is PluginFactoryAdapter || factories[i] is PluginFactoryAdapterForHeart) ? Color.LightYellow : Color.LightGray;
                btn.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;

                panel_buttons.Controls.Add(btn);
                yPos += buttonHeight + 5;
            }

            panel_buttons.AutoScroll = true;
        }

        private void BtnShape_Click(object? sender, EventArgs e)
        {
            Button? clickedBtn = sender as Button;
            currentFactory = clickedBtn?.Tag as IShapeFactory;
            currentFactory?.Reset();

            string shapeName = "";
            if (currentFactory is ShapeBaseFactory baseFactory)
                shapeName = baseFactory.ShapeName;
            else if (currentFactory is PluginFactoryAdapter adapter)
                shapeName = adapter.ShapeName;
            else if (currentFactory is PluginFactoryAdapterForHeart heartAdapter)
                shapeName = heartAdapter.ShapeName;

            statusLabel.Text = $"Drawing {shapeName}:\nMake clicks...";
        }

        private void panelPaint_Paint(object? sender, PaintEventArgs e)
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

        private void panel1_MouseClick(object? sender, MouseEventArgs e)
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
                            // Use Command Pattern for adding shapes
                            var addCommand = new AddShapeCommand(shapeList, newShape);
                            _commandManager.ExecuteCommand(addCommand);
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

        private void Settings_Click(object? sender, EventArgs e)
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

        private void SaveToFile_Click(object? sender, EventArgs e)
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

        private void LoadFromFile_Click(object? sender, EventArgs e)
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

        private Shape? DtoToShape(ShapeDto dto)
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

    /// <summary>
    /// Command for adding complex shapes (Builder Pattern result)
    /// </summary>
    public class AddComplexShapeCommand : ICommand
    {
        private ShapeList _shapeList;
        private ComplexShape _complexShape;
        private List<Shape> _addedShapes = new List<Shape>();

        public AddComplexShapeCommand(ShapeList shapeList, ComplexShape complexShape)
        {
            _shapeList = shapeList;
            _complexShape = complexShape;
        }

        public string Description => $"Added complex shape: {_complexShape.Name}";

        public void Execute()
        {
            foreach (var shape in _complexShape.Shapes)
            {
                _shapeList.Add(shape);
                _addedShapes.Add(shape);
            }
        }

        public void Undo()
        {
            foreach (var shape in _addedShapes)
            {
                _shapeList.ArrayShapes.Remove(shape);
                _shapeList.CountShapes--;
            }
            _addedShapes.Clear();
        }
    }

    /// <summary>
    /// Adapter wrapper for HeartPlugin (friend's plugin)
    /// </summary>
    public class PluginFactoryAdapterForHeart : IShapeFactory
    {
        private IShapeFactoryPlugin _pluginFactory;
        private List<(int X, int Y)> _clicks = new List<(int, int)>();

        public PluginFactoryAdapterForHeart(IShapeFactoryPlugin pluginFactory)
        {
            _pluginFactory = pluginFactory;
        }

        public string ShapeName => _pluginFactory.ShapeName;
        public int need_click_count => _pluginFactory.NeededClicks;
        public int index_now => _clicks.Count;

        public void GetCoordinates(int x, int y)
        {
            _clicks.Add((x, y));
            _pluginFactory.AddClick(x, y);
        }

        public bool IsReady() => _pluginFactory.IsReady();

        public void Reset()
        {
            _clicks.Clear();
            _pluginFactory.Reset();
        }

        public Shape CreateShape()
        {
            object shapeObject = _pluginFactory.CreateShape();
            return new PluginShapeWrapper(shapeObject, _pluginFactory.ShapeName, _pluginFactory);
        }
    }
}