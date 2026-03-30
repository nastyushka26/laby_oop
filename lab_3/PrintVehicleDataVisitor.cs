using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace lab_3
{
    public class PrintVehicleDataVisitor: IVisitor
    {
        // Store reference to RichTextBox
        private RichTextBox _richTextBox;

        // Constructor receives RichTextBox reference
        public PrintVehicleDataVisitor(RichTextBox richTextBox)
        {
            _richTextBox = richTextBox;
        }
        public void Visit(Airplane airplane)
        {
            _richTextBox.AppendText($"Flight Height: {airplane.HeightOfFlight}\n");
            _richTextBox.AppendText($"Motors: {airplane.MotorsCount}\n");
            return;
        }

        public void Visit(Bicycle bicycle)
        {
            _richTextBox.AppendText($"Hand Brakes: {(bicycle.HasHandBrakers ? "Yes" : "No")}\n");
            _richTextBox.AppendText($"Wheels: {bicycle.WheelsCount}\n");
            return;
        }

        public void Visit(Motorcycle motorcycle)
        {
            _richTextBox.AppendText($"Sport: {(motorcycle.isSport ? "Yes" : "No")}\n");
            _richTextBox.AppendText($"Wheels: {motorcycle.WheelsCount}\n");
            return;
        }

        public void Visit(Car car)
        {
            _richTextBox.AppendText($"Doors: {car.DoorsCount}\n");
            _richTextBox.AppendText($"Wheels: {car.WheelsCount}\n");
            return;
        }

        public void Visit(Boat boat)
        {
            _richTextBox.AppendText($"Size: {boat.Size}\n");
            _richTextBox.AppendText($"Material: {boat.Material}\n");
            return;
        }

        public void Visit(Liner liner)
        {
            _richTextBox.AppendText($"Size: {liner.Size}\n");
            _richTextBox.AppendText($"Floors: {liner.FloorsCount}\n");
            return;
        }
    }
}
