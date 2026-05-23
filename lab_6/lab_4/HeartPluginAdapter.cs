// HeartPluginAdapter.cs
using System;
using System.Drawing;
using System.Reflection;
using System.Text.Json;
using System.Windows.Forms;
using PluginInterface;
using Microsoft.VisualBasic;

namespace laba2oop
{
    /// <summary>
    /// Adapter Pattern - Converts friend's HeartPlugin to our IShapeFactoryPlugin interface
    /// </summary>
    public class HeartPluginAdapter : IShapeFactoryPlugin
    {
        private object _heartFactory;
        private int _clickX, _clickY;
        private bool _hasClick = false;
        private int _width = 60;
        private int _height = 55;
        private object _createdHeart = null;

        public HeartPluginAdapter(object heartFactory)
        {
            _heartFactory = heartFactory;
        }

        public string ShapeName => "Heart";

        public int NeededClicks => 1;

        public void AddClick(int x, int y)
        {
            if (!_hasClick)
            {
                _clickX = x;
                _clickY = y;
                _hasClick = true;
            }
        }

        public bool IsReady() => _hasClick;

        public object CreateShape()
        {
            try
            {
                // Show dialog for heart size
                string widthStr = Interaction.InputBox("Enter heart width:", "Create Heart", "60");
                string heightStr = Interaction.InputBox("Enter heart height:", "Create Heart", "55");

                int.TryParse(widthStr, out _width);
                int.TryParse(heightStr, out _height);

                // Call friend's factory method via reflection
                // Friend's method signature: CreateShape(Point startPoint, params object[] parameters)
                MethodInfo? method = _heartFactory.GetType().GetMethod("CreateShape");
                if (method != null)
                {
                    Point startPoint = new Point(_clickX, _clickY);

                    // Pass parameters as object array - this matches params object[] parameters
                    object[] parameters = new object[] { _width, _height, Color.Red };

                    // The method expects (Point, params object[])
                    _createdHeart = method.Invoke(_heartFactory, new object[] { startPoint, parameters });

                    return _createdHeart;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error creating heart: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            return null;
        }

        public void Reset() => _hasClick = false;

        public (int X, int Y)[] GetCoordinates()
        {
            return _hasClick ? new[] { (_clickX, _clickY) } : Array.Empty<(int, int)>();
        }

        public string SerializeShape(object shape)
        {
            var data = new { Width = _width, Height = _height };
            return JsonSerializer.Serialize(data);
        }

        public object DeserializeShape(string data, int defaultX, int defaultY)
        {
            _clickX = defaultX;
            _clickY = defaultY;
            _hasClick = true;

            try
            {
                var dto = JsonSerializer.Deserialize<HeartDto>(data);
                if (dto != null)
                {
                    _width = dto.Width;
                    _height = dto.Height;
                }
            }
            catch { }

            return CreateShape();
        }

        private class HeartDto
        {
            public int Width { get; set; }
            public int Height { get; set; }
        }

        /// <summary>
        /// Get heart points from the created heart object for drawing
        /// </summary>
        public PointF[] GetHeartPoints()
        {
            if (_createdHeart == null) return GenerateHeartPointsFallback();

            try
            {
                var method = _createdHeart.GetType().GetMethod("GetHeartPoints");
                if (method != null)
                {
                    var result = method.Invoke(_createdHeart, null);
                    if (result is PointF[] points)
                    {
                        return points;
                    }
                }
            }
            catch { }

            return GenerateHeartPointsFallback();
        }

        /// <summary>
        /// Generate heart points mathematically (fallback if friend's method fails)
        /// </summary>
        private PointF[] GenerateHeartPointsFallback()
        {
            PointF[] points = new PointF[36];
            float cx = _clickX;
            float cy = _clickY;
            float scale = Math.Min(_width, _height) / 35f;

            for (int i = 0; i <= 35; i++)
            {
                double t = i * 2 * Math.PI / 35;
                double x = 16 * Math.Pow(Math.Sin(t), 3);
                double y = 13 * Math.Cos(t) - 5 * Math.Cos(2 * t) - 2 * Math.Cos(3 * t) - Math.Cos(4 * t);
                points[i] = new PointF(cx + (float)(x * scale), cy - (float)(y * scale));
            }

            return points;
        }
    }

    /// <summary>
    /// Plugin entry adapter for friend's HeartPlugin
    /// </summary>
    public class HeartPluginEntryAdapter : IShapePlugin
    {
        private IShapeFactoryPlugin? _factory;

        public string PluginId => "com.friend.heart.v1";
        public string PluginName => "Heart Shape Plugin (Friend)";
        public string Version => "1.0.0";

        public bool Initialize()
        {
            try
            {
                string pluginPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Plugins", "HeartPlugin.dll");

                if (!File.Exists(pluginPath))
                {
                    pluginPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "HeartPlugin.dll");
                }

                if (!File.Exists(pluginPath))
                {
                    MessageBox.Show("HeartPlugin.dll not found!", "Plugin Error",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return false;
                }

                Assembly assembly = Assembly.LoadFrom(pluginPath);
                Type? factoryType = assembly.GetType("HeartPlugin.HeartFactory");

                if (factoryType != null)
                {
                    object? factoryInstance = Activator.CreateInstance(factoryType);
                    if (factoryInstance != null)
                    {
                        _factory = new HeartPluginAdapter(factoryInstance);
                        return true;
                    }
                }
                return false;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Failed to load HeartPlugin: {ex.Message}", "Plugin Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }

        public IShapeFactoryPlugin? GetShapeFactory() => _factory;
    }
}