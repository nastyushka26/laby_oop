using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Bson.Serialization.Attributes;

namespace lab_3
{
    [BsonKnownTypes(typeof(LandVehicle), typeof(WaterVehicle), typeof(AirVehicle))]
    public abstract class Vehicle
    {
        // determine fields that will be serialized
        [BsonElement("Brand")]
        public string Brand;
        [BsonElement("Color")]
        public string Color;
        [BsonElement("MaxSpeed")]
        public int MaxSpeed;
        [BsonElement("ProductYear")]
        public int ProductYear;
        
        // constructor without parametrs for deserialization
        public Vehicle() { }
        public abstract void Accept(IVisitor visitor);   // for visitor acception

        // constructor to create vehicle object
        public Vehicle(string brand, string color, int productYear, int maxSpeed)
        {
            Brand = brand;
            Color = color;
            MaxSpeed = maxSpeed;
            ProductYear = productYear;
        }

        // mehtod for implementing by each class of vehicle - moving
        public abstract void Move();

        // method that can be overriden - stopping
        public virtual void Stop()
        {
            Console.WriteLine("Транспорт остановился\n");
        }

        // method for getting information about the vehicle
        public virtual string GetInfo()
        {
            return $"Бренд: {Brand}, Цвет: {Color}, Год: {ProductYear}, Скорость: {MaxSpeed}";
        }
    }
}
