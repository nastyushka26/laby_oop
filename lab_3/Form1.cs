using MongoDB.Bson.IO;
using MongoDB.Bson.Serialization;

namespace lab_3
{
    public partial class Form1 : Form
    {
        private List<Vehicle> vehicles = new List<Vehicle>();

        private string fileName = "..\\transport.bson";
        public Form1()
        {
            InitializeComponent();
            // Check if file exists and load data
            if (File.Exists(fileName) && new FileInfo(fileName).Length > 0)
            {
                try
                {
                    using (FileStream stream = new FileStream(fileName, FileMode.Open))
                    using (BsonBinaryReader reader = new BsonBinaryReader(stream))
                    {
                        var container = BsonSerializer.Deserialize<VehicleList>(reader);
                        vehicles = container.Vehicles ?? new List<Vehicle>();
                    }
                }
                catch
                {
                    // If file is corrupted, start with empty list
                    vehicles = new List<Vehicle>();
                }
            }
            UpdateListBox();
        }

        private void UpdateListBox()
        {
            listBox1.Items.Clear();
            Vehicle v;
            for (int i = 0; i < vehicles.Count; i++)
            {
                v = vehicles[i];
                // using ToString() or GetInfo()
                listBox1.Items.Add($"{v.GetType().Name}: {v.Brand} ({v.Color})");
            }
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            using (var form = new VehicleForm())
            {
                if (form.ShowDialog() == DialogResult.OK)
                {
                    vehicles.Add(form.Vehicle);
                    UpdateListBox();
                }
            }
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listBox1.SelectedIndex >= 0)
            {
                ShowVehicleInfo(vehicles[listBox1.SelectedIndex]);
            }
        }

        private void ShowVehicleInfo(Vehicle vehicle)
        {
            richTextBox1.Clear();

            // Using polymorphism to display specific information
            richTextBox1.AppendText($"Brand: {vehicle.Brand}\n");
            richTextBox1.AppendText($"Color: {vehicle.Color}\n");
            richTextBox1.AppendText($"Year: {vehicle.ProductYear}\n");
            richTextBox1.AppendText($"Max Speed: {vehicle.MaxSpeed}\n");

            // Show specific properties based on vehicle type
            switch (vehicle)
            {
                case Car car:
                    richTextBox1.AppendText($"Doors: {car.DoorsCount}\n");
                    richTextBox1.AppendText($"Wheels: {car.WheelsCount}\n");
                    break;
                case Motorcycle motorcycle:
                    richTextBox1.AppendText($"Sport: {(motorcycle.isSport ? "Yes" : "No")}\n");
                    richTextBox1.AppendText($"Wheels: {motorcycle.WheelsCount}\n");
                    break;
                case Bicycle bicycle:
                    richTextBox1.AppendText($"Hand Brakes: {(bicycle.HasHandBrakers ? "Yes" : "No")}\n");
                    richTextBox1.AppendText($"Wheels: {bicycle.WheelsCount}\n");
                    break;
                case Airplane airplane:
                    richTextBox1.AppendText($"Flight Height: {airplane.HeightOfFlight}\n");
                    richTextBox1.AppendText($"Motors: {airplane.MotorsCount}\n");
                    break;
                case Boat boat:
                    richTextBox1.AppendText($"Size: {boat.Size}\n");
                    richTextBox1.AppendText($"Material: {boat.Material}\n");
                    break;
                case Liner liner:
                    richTextBox1.AppendText($"Size: {liner.Size}\n");
                    richTextBox1.AppendText($"Floors: {liner.FloorsCount}\n");
                    break;
            }
        }

        private void btnRemove_Click(object sender, EventArgs e)
        {
            if (listBox1.SelectedIndex >= 0)
            {
                vehicles.RemoveAt(listBox1.SelectedIndex);
                UpdateListBox();
                richTextBox1.Clear();
            }
        }

        private void btnChange_Click(object sender, EventArgs e)
        {
            if (listBox1.SelectedIndex >= 0)
            {
                using (var form = new VehicleForm(vehicles[listBox1.SelectedIndex]))
                {
                    if (form.ShowDialog() == DialogResult.OK)
                    {
                        vehicles[listBox1.SelectedIndex] = form.Vehicle;
                        UpdateListBox();
                        ShowVehicleInfo(form.Vehicle);
                    }
                }
            }
        }

        // event handler method for saving list objects to file
        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                // Create a container for the list
                var container = new VehicleList(vehicles);

                using (FileStream stream = new FileStream(fileName, FileMode.Create))
                using (BsonBinaryWriter writer = new BsonBinaryWriter(stream))
                {
                    // serializing container to Bson
                    BsonSerializer.Serialize(writer, container);
                }
                MessageBox.Show($"Saved {vehicles.Count} vehicles to {fileName}");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error saving: {ex.Message}");
            }
        }

        // method for loading list objects from file
        private void btnLoad_Click(object sender, EventArgs e)
        {
            try
            {
                if (File.Exists(fileName))
                {
                    // Check if file is empty
                    if (new FileInfo(fileName).Length == 0)
                    {
                        vehicles = new List<Vehicle>();
                        MessageBox.Show("File is empty. Starting with empty list.");
                        UpdateListBox();
                        return;
                    }

                    using (FileStream stream = new FileStream(fileName, FileMode.Open))
                    using (BsonBinaryReader reader = new BsonBinaryReader(stream))
                    {
                        // deserializing container from Bson
                        var container = BsonSerializer.Deserialize<VehicleList>(reader);
                        vehicles = container.Vehicles ?? new List<Vehicle>();
                    }
                    UpdateListBox();
                    MessageBox.Show($"Loaded {vehicles.Count} vehicles from {fileName}");
                }
                else
                {
                    MessageBox.Show("File not found!");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading: {ex.Message}");
                // Initialize with empty list on error
                vehicles = new List<Vehicle>();
                UpdateListBox();
            }
        }
    }
}
