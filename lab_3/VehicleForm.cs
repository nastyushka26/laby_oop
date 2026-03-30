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
        private Dictionary<string, Control> specificControls = new Dictionary<string, Control>();
        private IVehicleFactory currentFactory;
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

        private void UpdateSpecificControls()
        {
            panelSpecific.Controls.Clear();
            specificControls.Clear();

            string selectedType = CmbType.SelectedItem?.ToString();
            if (string.IsNullOrEmpty(selectedType)) return;

            currentFactory = VehicleFactoryRegistry.GetFactory(selectedType);
            if (currentFactory == null) return;

            Control[] controls = currentFactory.GetSpecificControls();

            foreach (Control control in controls)
            {
                panelSpecific.Controls.Add(control);
                if (!string.IsNullOrEmpty(control.Name))
                {
                    specificControls[control.Name] = control;
                }
            }
        }

        // event handler for button "Save" click
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

                string selectedType = CmbType.SelectedItem?.ToString();
                if (string.IsNullOrEmpty(selectedType))
                {
                    MessageBox.Show("Please select vehicle type!");
                    return;
                }

                currentFactory = VehicleFactoryRegistry.GetFactory(selectedType);
                if (currentFactory == null)
                {
                    MessageBox.Show("Unknown vehicle type!");
                    return;
                }

                // Build parameters dictionary
                var parameters = new Dictionary<string, object>
                {
                    ["Brand"] = txtBrand.Text,
                    ["Color"] = txtColor.Text,
                    ["Year"] = year,
                    ["MaxSpeed"] = maxSpeed
                };

                // Extract specific parameters based on control types
                foreach (var kvp in specificControls)
                {
                    Control control = kvp.Value;
                    string value = null;

                    if (control is TextBox textBox)
                    {
                        value = textBox.Text;
                        if (control.Name.Contains("Wheels") || control.Name.Contains("Height") ||
                            control.Name.Contains("Motors") || control.Name.Contains("Doors") ||
                            control.Name.Contains("Floors"))
                        {
                            if (!int.TryParse(value, out int intValue))
                            {
                                MessageBox.Show($"Please enter valid number in {control.Name}");
                                return;
                            }
                            parameters[control.Name.Replace("txt", "")] = intValue;
                        }
                        else
                        {
                            parameters[control.Name.Replace("txt", "")] = value;
                        }
                    }
                    else if (control is CheckBox checkBox)
                    {
                        parameters[control.Name.Replace("chk", "")] = checkBox.Checked;
                    }
                    else if (control is ComboBox comboBox)
                    {
                        parameters[control.Name.Replace("cmb", "")] = comboBox.SelectedItem?.ToString();
                    }
                }

                vehicle = currentFactory.CreateVehicle(parameters);

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

        private void LoadVehicleData()
        {
            if (vehicle == null) return;

            // Load common data
            txtBrand.Text = vehicle.Brand;
            txtColor.Text = vehicle.Color;
            txtYear.Text = vehicle.ProductYear.ToString();
            txtMaxSpeed.Text = vehicle.MaxSpeed.ToString();

            // Find factory for this vehicle type
            string typeName = vehicle.GetType().Name;
            currentFactory = VehicleFactoryRegistry.GetFactory(typeName);

            if (currentFactory != null)
            {
                CmbType.SelectedItem = typeName;
                CmbType_SelectedIndexChanged(null, null);
                currentFactory.LoadSpecificData(vehicle, specificControls);
            }
        }

        private void CmbType_SelectedIndexChanged(object sender, EventArgs e)
        {
            UpdateSpecificControls();
        }
    }
}
