using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace lab_3
{
    public partial class VehicleForm : Form
    {
        private Vehicle vehicle;
        private TextBox txtWheels, txtDoors, txtHeight, txtMotors;
        private TextBox txtFloors, txtMaterial;
        CheckBox chkHandBrakes, chkIsSport;
        public Vehicle Vehicle { get { return vehicle; } }
        public VehicleForm(Vehicle existingVehicle = null)
        {
            InitializeComponent();
            this.vehicle = existingVehicle;
            // Properly fill the combobox
            CmbType.Items.Clear();
            CmbType.Items.AddRange(new object[] { "Car", "Motorcycle", "Bicycle", "Airplane", "Boat", "Liner" });

            CmbType.SelectedIndex = 0; // Select first item by default

            // Add cancel button handler
            btnCancel.Click += (s, e) => this.DialogResult = DialogResult.Cancel;

            if (existingVehicle != null)
            {
                LoadVehicleData();
            }
            else
            {
                // Show default controls for first type
                CmbType_SelectedIndexChanged(null, null);
            }
        }

        private void AddCarControls()
        {
            var lblWheels = new Label { Text = "Wheels:", Location = new System.Drawing.Point(10, 10), Width = 100 };
            txtWheels = new TextBox { Location = new System.Drawing.Point(120, 10), Width = 150 };
            txtWheels.Text = "4";

            var lblDoors = new Label { Text = "Doors:", Location = new System.Drawing.Point(10, 70), Width = 100 };
            txtDoors = new TextBox { Location = new System.Drawing.Point(120, 80), Width = 150 };
            txtDoors.Text = "4";

            panelSpecific.Controls.AddRange(new Control[] { lblWheels, txtWheels, lblDoors, txtDoors });
        }

        private void AddMotorcycleControls()
        {
            var lblWheels = new Label { Text = "Wheels:", Location = new System.Drawing.Point(10, 10), Width = 100 };
            txtWheels = new TextBox { Location = new System.Drawing.Point(120, 10), Width = 150 };
            txtWheels.Text = "2";

            var lblIsSport = new Label { Text = "Sport:", Location = new System.Drawing.Point(10, 70), Width = 100 };
            chkIsSport = new CheckBox { Location = new System.Drawing.Point(120, 80), Width = 150 };

            panelSpecific.Controls.AddRange(new Control[] { lblWheels, txtWheels, lblIsSport, chkIsSport });
        }

        private void AddBicycleControls()
        {
            var lblWheels = new Label { Text = "Wheels:", Location = new System.Drawing.Point(10, 10), Width = 100 };
            txtWheels = new TextBox { Location = new System.Drawing.Point(120, 10), Width = 150 };
            txtWheels.Text = "2";

            var lblHandBrakes = new Label { Text = "Hand brakes:", Location = new System.Drawing.Point(10, 70), Width = 100 };
            chkHandBrakes = new CheckBox { Location = new System.Drawing.Point(120, 80), Width = 150 };

            panelSpecific.Controls.AddRange(new Control[] { lblWheels, txtWheels, lblHandBrakes, chkHandBrakes });
        }

        private void AddAirplaneControls()
        {
            var lblHeight = new Label { Text = "Flight height:", Location = new System.Drawing.Point(10, 10), Width = 100 };
            txtHeight = new TextBox { Location = new System.Drawing.Point(120, 10), Width = 150 };
            txtHeight.Text = "10000";

            var lblMotors = new Label { Text = "Motors:", Location = new System.Drawing.Point(10, 70), Width = 100 };
            txtMotors = new TextBox { Location = new System.Drawing.Point(120, 80), Width = 150 };
            txtMotors.Text = "2";

            panelSpecific.Controls.AddRange(new Control[] { lblHeight, txtMotors });
        }

        private void AddBoatControls()
        {
            var lblSize = new Label { Text = "Size:", Location = new Point(10, 10), Width = 100 };
            var cmbSize = new ComboBox
            {
                Location = new Point(120, 10),
                Width = 150,
                Name = "cmbSize",
                DropDownStyle = ComboBoxStyle.DropDownList
            };
            cmbSize.Items.AddRange(new object[] { "small", "medium", "large" });
            cmbSize.SelectedIndex = 1;

            var lblMaterial = new Label { Text = "Material:", Location = new Point(10, 70), Width = 100 };
            txtMaterial = new TextBox { Location = new Point(120, 80), Width = 150, Name = "txtMaterial" };
            txtMaterial.Text = "wood";

            panelSpecific.Controls.AddRange(new Control[] { lblSize, cmbSize, lblMaterial, txtMaterial });
        }

        private void AddLinerControls()
        {
            var lblSize = new Label { Text = "Size:", Location = new Point(10, 10), Width = 100 };
            var cmbSize = new ComboBox
            {
                Location = new Point(120, 10),
                Width = 150,
                Name = "cmbSize",
                DropDownStyle = ComboBoxStyle.DropDownList
            };
            cmbSize.Items.AddRange(new object[] { "small", "medium", "large" });
            cmbSize.SelectedIndex = 2;

            var lblFloors = new Label { Text = "Floors:", Location = new Point(10, 70), Width = 100 };
            txtFloors = new TextBox { Location = new Point(120, 80), Width = 150, Name = "txtFloors" };
            txtFloors.Text = "10";

            panelSpecific.Controls.AddRange(new Control[] { lblSize, cmbSize, lblFloors, txtFloors });
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                // Validate common fields
                if (string.IsNullOrWhiteSpace(txtBrand.Text) ||
                    string.IsNullOrWhiteSpace(txtColor.Text) ||
                    !int.TryParse(txtYear.Text, out int year) ||
                    !int.TryParse(txtMaxSpeed.Text, out int maxSpeed))
                {
                    MessageBox.Show("Please fill all common fields correctly!");
                    return;
                }

                // Create vehicle based on type
                string type = CmbType.SelectedItem?.ToString();
                if (string.IsNullOrEmpty(type))
                {
                    MessageBox.Show("Please select vehicle type!");
                    return;
                }

                switch (type)
                {
                    case "Car":
                        if (ValidateIntFields(txtWheels, txtDoors))
                        {
                            vehicle = new Car(txtBrand.Text, txtColor.Text, year, maxSpeed,
                                int.Parse(txtWheels.Text), int.Parse(txtDoors.Text));
                        }
                        break;

                    case "Motorcycle":
                        if (ValidateIntFields(txtWheels))
                        {
                            vehicle = new Motorcycle(txtBrand.Text, txtColor.Text, year, maxSpeed,
                                int.Parse(txtWheels.Text), chkIsSport.Checked);
                        }
                        break;

                    case "Bicycle":
                        if (ValidateIntFields(txtWheels))
                        {
                            vehicle = new Bicycle(txtBrand.Text, txtColor.Text, year, maxSpeed,
                                int.Parse(txtWheels.Text), chkHandBrakes.Checked);
                        }
                        break;

                    case "Airplane":
                        if (ValidateIntFields(txtHeight, txtMotors))
                        {
                            vehicle = new Airplane(txtBrand.Text, txtColor.Text, year, maxSpeed,
                                int.Parse(txtHeight.Text), int.Parse(txtMotors.Text));
                        }
                        break;

                    case "Boat":
                        // Find the size combobox in panelSpecific
                        ComboBox cmbSize = FindControl<ComboBox>("cmbSize");
                        if (cmbSize != null && !string.IsNullOrWhiteSpace(txtMaterial.Text))
                        {
                            vehicle = new Boat(txtBrand.Text, txtColor.Text, year, maxSpeed,
                                cmbSize.SelectedItem.ToString(), txtMaterial.Text);
                        }
                        else
                        {
                            MessageBox.Show("Please fill all boat fields!");
                            return;
                        }
                        break;

                    case "Liner":
                        // Find the size combobox in panelSpecific
                        ComboBox cmbLinerSize = FindControl<ComboBox>("cmbSize");
                        if (cmbLinerSize != null && ValidateIntFields(txtFloors))
                        {
                            vehicle = new Liner(txtBrand.Text, txtColor.Text, year, maxSpeed,
                                cmbLinerSize.SelectedItem.ToString(), int.Parse(txtFloors.Text));
                        }
                        else
                        {
                            MessageBox.Show("Please fill all liner fields!");
                            return;
                        }
                        break;
                }

                if (vehicle != null)
                {
                    this.DialogResult = DialogResult.OK;
                    this.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}");
            }
        }

        private bool ValidateIntFields(params TextBox[] fields)
        {
            foreach (var field in fields)
            {
                if (!int.TryParse(field.Text, out _))
                {
                    MessageBox.Show($"Please enter valid number in {field.Name}");
                    return false;
                }
            }
            return true;
        }

        private void LoadVehicleData()
        {
            if (vehicle == null) return;

            // Load common data
            txtBrand.Text = vehicle.Brand;
            txtColor.Text = vehicle.Color;
            txtYear.Text = vehicle.ProductYear.ToString();
            txtMaxSpeed.Text = vehicle.MaxSpeed.ToString();

            // Set type and load specific data
            if (vehicle is Car car)
            {
                CmbType.SelectedItem = "Car";
                CmbType_SelectedIndexChanged(null, null);
                txtWheels.Text = car.WheelsCount.ToString();
                txtDoors.Text = car.DoorsCount.ToString();
            }
            else if (vehicle is Motorcycle motorcycle)
            {
                CmbType.SelectedItem = "Motorcycle";
                CmbType_SelectedIndexChanged(null, null);
                txtWheels.Text = motorcycle.WheelsCount.ToString();
                chkIsSport.Checked = motorcycle.isSport;
            }
            else if (vehicle is Bicycle bicycle)
            {
                CmbType.SelectedItem = "Bicycle";
                CmbType_SelectedIndexChanged(null, null);
                txtWheels.Text = bicycle.WheelsCount.ToString();
                chkHandBrakes.Checked = bicycle.HasHandBrakers;
            }
            else if (vehicle is Airplane airplane)
            {
                CmbType.SelectedItem = "Airplane";
                CmbType_SelectedIndexChanged(null, null);
                txtHeight.Text = airplane.HeightOfFlight.ToString();
                txtMotors.Text = airplane.MotorsCount.ToString();
            }
            else if (vehicle is Boat boat)
            {
                CmbType.SelectedItem = "Boat";
                CmbType_SelectedIndexChanged(null, null);
                ComboBox cmbSize = FindControl<ComboBox>("cmbSize");
                if (cmbSize != null) cmbSize.SelectedItem = boat.Size;
                txtMaterial.Text = boat.Material;
            }
            else if (vehicle is Liner liner)
            {
                CmbType.SelectedItem = "Liner";
                CmbType_SelectedIndexChanged(null, null);
                ComboBox cmbSize = FindControl<ComboBox>("cmbSize");
                if (cmbSize != null) cmbSize.SelectedItem = liner.Size;
                txtFloors.Text = liner.FloorsCount.ToString();
            }
        }

        private void CmbType_SelectedIndexChanged(object sender, EventArgs e)
        {
            panelSpecific.Controls.Clear();

            if (CmbType.SelectedItem == null) return;

            switch (CmbType.SelectedItem.ToString())
            {
                case "Car":
                    AddCarControls();
                    break;
                case "Motorcycle":
                    AddMotorcycleControls();
                    break;
                case "Bicycle":
                    AddBicycleControls();
                    break;
                case "Airplane":
                    AddAirplaneControls();
                    break;
                case "Boat":
                    AddBoatControls();
                    break;
                case "Liner":
                    AddLinerControls();
                    break;
            }
        }

        // Helper method to find a control in panelSpecific by name
        private T FindControl<T>(string name) where T : Control
        {
            foreach (Control control in panelSpecific.Controls)
            {
                if (control.Name == name && control is T)
                    return (T)control;
            }
            return null;
        }
    }
}
