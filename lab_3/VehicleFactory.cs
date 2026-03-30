namespace lab_3
{
    // Factory interface for creating vehicles
    public interface IVehicleFactory
    {
        Vehicle CreateVehicle(Dictionary<string, object> parameters);
        Control[] GetSpecificControls();
        string GetTypeName();
        void LoadSpecificData(Vehicle vehicle, Dictionary<string, Control> controls);
    }

    // Factory for Car
    public class CarFactory : IVehicleFactory
    {
        public Vehicle CreateVehicle(Dictionary<string, object> parameters)
        {
            return new Car(
                (string)parameters["Brand"],
                (string)parameters["Color"],
                (int)parameters["Year"],
                (int)parameters["MaxSpeed"],
                (int)parameters["Wheels"],
                (int)parameters["Doors"]
            );
        }

        public Control[] GetSpecificControls()
        {
            var lblWheels = new Label { Text = "Wheels:", Location = new System.Drawing.Point(10, 30), Height = 32, Width = 100 };
            var txtWheels = new TextBox { Location = new System.Drawing.Point(120, 30), Width = 150, Height = 32, Name = "txtWheels" };
            txtWheels.Text = "4";

            var lblDoors = new Label { Text = "Doors:", Location = new System.Drawing.Point(10, 90), Width = 100, Height = 32 };
            var txtDoors = new TextBox { Location = new System.Drawing.Point(120, 90), Width = 150, Height = 32, Name = "txtDoors" };
            txtDoors.Text = "4";

            return new Control[] { lblWheels, txtWheels, lblDoors, txtDoors };
        }

        public string GetTypeName() => "Car";

        public void LoadSpecificData(Vehicle vehicle, Dictionary<string, Control> controls)
        {
            var car = vehicle as Car;
            if (car != null)
            {
                ((TextBox)controls["txtWheels"]).Text = car.WheelsCount.ToString();
                ((TextBox)controls["txtDoors"]).Text = car.DoorsCount.ToString();
            }
        }
    }

    // Factory for Motorcycle
    public class MotorcycleFactory : IVehicleFactory
    {
        public Vehicle CreateVehicle(Dictionary<string, object> parameters)
        {
            return new Motorcycle(
                (string)parameters["Brand"],
                (string)parameters["Color"],
                (int)parameters["Year"],
                (int)parameters["MaxSpeed"],
                (int)parameters["Wheels"],
                (bool)parameters["IsSport"]
            );
        }

        public Control[] GetSpecificControls()
        {
            var lblWheels = new Label { Text = "Wheels:", Location = new System.Drawing.Point(10, 30), Width = 100, Height = 32 };
            var txtWheels = new TextBox { Location = new System.Drawing.Point(120, 30), Width = 70, Height = 32, Name = "txtWheels" };
            txtWheels.Text = "2";

            var lblIsSport = new Label { Text = "Sport:", Location = new System.Drawing.Point(10, 90), Width = 100, Height = 32 };
            var chkIsSport = new CheckBox { Location = new System.Drawing.Point(120, 95), Width = 150, Height = 32, Name = "chkIsSport" };

            return new Control[] { lblWheels, txtWheels, lblIsSport, chkIsSport };
        }

        public string GetTypeName() => "Motorcycle";

        public void LoadSpecificData(Vehicle vehicle, Dictionary<string, Control> controls)
        {
            var motorcycle = vehicle as Motorcycle;
            if (motorcycle != null)
            {
                ((TextBox)controls["txtWheels"]).Text = motorcycle.WheelsCount.ToString();
                ((CheckBox)controls["chkIsSport"]).Checked = motorcycle.isSport;
            }
        }
    }

    // Factory for Bicycle
    public class BicycleFactory : IVehicleFactory
    {
        public Vehicle CreateVehicle(Dictionary<string, object> parameters)
        {
            return new Bicycle(
                (string)parameters["Brand"],
                (string)parameters["Color"],
                (int)parameters["Year"],
                (int)parameters["MaxSpeed"],
                (int)parameters["Wheels"],
                (bool)parameters["HandBrakes"]
            );
        }

        public Control[] GetSpecificControls()
        {
            var lblWheels = new Label { Text = "Wheels:", Location = new System.Drawing.Point(10, 30), Height = 32, Width = 100 };
            var txtWheels = new TextBox { Location = new System.Drawing.Point(120, 30), Width = 100, Height = 32, Name = "txtWheels" };
            txtWheels.Text = "2";

            var lblHandBrakes = new Label { Text = "Hand brakes:", Location = new System.Drawing.Point(10, 90), Width = 170, Height = 32 };
            var chkHandBrakes = new CheckBox { Location = new System.Drawing.Point(190, 95), Width = 150, Height = 32, Name = "chkHandBrakes" };

            return new Control[] { lblWheels, txtWheels, lblHandBrakes, chkHandBrakes };
        }

        public string GetTypeName() => "Bicycle";

        public void LoadSpecificData(Vehicle vehicle, Dictionary<string, Control> controls)
        {
            var bicycle = vehicle as Bicycle;
            if (bicycle != null)
            {
                ((TextBox)controls["txtWheels"]).Text = bicycle.WheelsCount.ToString();
                ((CheckBox)controls["chkHandBrakes"]).Checked = bicycle.HasHandBrakers;
            }
        }
    }

    // Factory for Airplane
    public class AirplaneFactory : IVehicleFactory
    {
        public Vehicle CreateVehicle(Dictionary<string, object> parameters)
        {
            return new Airplane(
                (string)parameters["Brand"],
                (string)parameters["Color"],
                (int)parameters["Year"],
                (int)parameters["MaxSpeed"],
                (int)parameters["Height"],
                (int)parameters["Motors"]
            );
        }

        public Control[] GetSpecificControls()
        {
            var lblHeight = new Label { Text = "Flight height:", Location = new System.Drawing.Point(10, 30), Width = 100, Height = 32 };
            var txtHeight = new TextBox { Location = new System.Drawing.Point(120, 30), Width = 150, Height = 32, Name = "txtHeight" };
            txtHeight.Text = "10000";

            var lblMotors = new Label { Text = "Motors:", Location = new System.Drawing.Point(10, 90), Width = 100, Height = 32 };
            var txtMotors = new TextBox { Location = new System.Drawing.Point(120, 90), Width = 150, Height = 32, Name = "txtMotors" };
            txtMotors.Text = "2";

            return new Control[] { lblHeight, txtHeight, lblMotors, txtMotors };
        }

        public string GetTypeName() => "Airplane";

        public void LoadSpecificData(Vehicle vehicle, Dictionary<string, Control> controls)
        {
            var airplane = vehicle as Airplane;
            if (airplane != null)
            {
                ((TextBox)controls["txtHeight"]).Text = airplane.HeightOfFlight.ToString();
                ((TextBox)controls["txtMotors"]).Text = airplane.MotorsCount.ToString();
            }
        }
    }

    // Factory for Boat
    public class BoatFactory : IVehicleFactory
    {
        public Vehicle CreateVehicle(Dictionary<string, object> parameters)
        {
            return new Boat(
                (string)parameters["Brand"],
                (string)parameters["Color"],
                (int)parameters["Year"],
                (int)parameters["MaxSpeed"],
                (string)parameters["Size"],
                (string)parameters["Material"]
            );
        }

        public Control[] GetSpecificControls()
        {
            var lblSize = new Label { Text = "Size:", Location = new System.Drawing.Point(10, 30), Width = 120, Height = 32 };
            var cmbSize = new ComboBox
            {
                Location = new System.Drawing.Point(130, 30),
                Width = 150,
                Height = 32,
                Name = "cmbSize",
                DropDownStyle = ComboBoxStyle.DropDownList
            };
            cmbSize.Items.AddRange(new object[] { "small", "medium", "large" });
            cmbSize.SelectedIndex = 1;

            var lblMaterial = new Label { Text = "Material:", Location = new System.Drawing.Point(10, 90), Width = 120, Height = 32 };
            var txtMaterial = new TextBox { Location = new System.Drawing.Point(130, 90), Width = 150, Height = 32, Name = "txtMaterial" };
            txtMaterial.Text = "wood";

            return new Control[] { lblSize, cmbSize, lblMaterial, txtMaterial };
        }

        public string GetTypeName() => "Boat";

        public void LoadSpecificData(Vehicle vehicle, Dictionary<string, Control> controls)
        {
            var boat = vehicle as Boat;
            if (boat != null)
            {
                ((ComboBox)controls["cmbSize"]).SelectedItem = boat.Size;
                ((TextBox)controls["txtMaterial"]).Text = boat.Material;
            }
        }
    }

    // Factory for Liner
    public class LinerFactory : IVehicleFactory
    {
        public Vehicle CreateVehicle(Dictionary<string, object> parameters)
        {
            return new Liner(
                (string)parameters["Brand"],
                (string)parameters["Color"],
                (int)parameters["Year"],
                (int)parameters["MaxSpeed"],
                (string)parameters["Size"],
                (int)parameters["Floors"]
            );
        }

        public Control[] GetSpecificControls()
        {
            var lblSize = new Label { Text = "Size:", Location = new System.Drawing.Point(10, 30), Width = 100, Height = 32 };
            var cmbSize = new ComboBox
            {
                Location = new System.Drawing.Point(110, 30),
                Width = 150,
                Height = 32,
                Name = "cmbSize",
                DropDownStyle = ComboBoxStyle.DropDownList
            };
            cmbSize.Items.AddRange(new object[] { "small", "medium", "large" });
            cmbSize.SelectedIndex = 2;

            var lblFloors = new Label { Text = "Floors:", Location = new System.Drawing.Point(10, 90), Width = 100, Height = 32 };
            var txtFloors = new TextBox { Location = new System.Drawing.Point(110, 90), Width = 150, Height = 32, Name = "txtFloors" };
            txtFloors.Text = "10";

            return new Control[] { lblSize, cmbSize, lblFloors, txtFloors };
        }

        public string GetTypeName() => "Liner";

        public void LoadSpecificData(Vehicle vehicle, Dictionary<string, Control> controls)
        {
            var liner = vehicle as Liner;
            if (liner != null)
            {
                ((ComboBox)controls["cmbSize"]).SelectedItem = liner.Size;
                ((TextBox)controls["txtFloors"]).Text = liner.FloorsCount.ToString();
            }
        }
    }

    // Registry for factories - adding new vehicle type only requires registering new factory here
    public static class VehicleFactoryRegistry
    {
        private static readonly Dictionary<string, IVehicleFactory> _factories = new Dictionary<string, IVehicleFactory>();

        static VehicleFactoryRegistry()
        {
            // Register all factories - adding new type means adding one line here
            RegisterFactory(new CarFactory());
            RegisterFactory(new MotorcycleFactory());
            RegisterFactory(new BicycleFactory());
            RegisterFactory(new AirplaneFactory());
            RegisterFactory(new BoatFactory());
            RegisterFactory(new LinerFactory());
        }

        public static void RegisterFactory(IVehicleFactory factory)
        {
            _factories[factory.GetTypeName()] = factory;
        }

        public static IVehicleFactory GetFactory(string typeName)
        {
            return _factories.ContainsKey(typeName) ? _factories[typeName] : null;
        }

        public static string[] GetAllTypeNames()
        {
            return _factories.Keys.ToArray();
        }
    }
}