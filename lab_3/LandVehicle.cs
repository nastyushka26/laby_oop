using MongoDB.Bson.Serialization.Attributes;

namespace lab_3
{
    // determine landVehicle types for bson serialization
    [BsonKnownTypes(typeof(Car), typeof(Motorcycle), typeof(Bicycle))]
    
    public class LandVehicle : Vehicle
    {
        [BsonElement("WheelCount")]
        public int WheelsCount;

        // constructor for deserialization
        public LandVehicle() { }

        // constructor for creating landVehicle objects
        public LandVehicle(string brand, string color, int year, int maxSpeed, int wheelCount)
        : base(brand, color, year, maxSpeed)
        {
            WheelsCount = wheelCount;
        }

        // method for land vehicles - they drive
        public override void Move()
        {
            Console.WriteLine($"Еду по земле на {WheelsCount} колесах\n");
        }
    }

    public class Car : LandVehicle
    {
        [BsonElement("DoorsCount")]
        public int DoorsCount;

        // constructor for deserialization
        public Car() { }

        // constructor for creating car objects
        public Car(string brand, string color, int year, int maxSpeed, int wheelCount, int doorsCount) 
            : base(brand, color, year, maxSpeed, wheelCount)
        {
            DoorsCount = doorsCount;
        }

        // moving method for car
        public override void Move()
        {
            Console.WriteLine($"Машина едет\n");
        }

    }

    public class Motorcycle : LandVehicle
    {
        [BsonElement("isSport")]
        public bool isSport;

        // constructor for deserialization
        public Motorcycle() { }

        // constructor for creating motorcycle objects
        public Motorcycle(string brand, string color, int year, int maxSpeed, int wheelCount, bool isSport)
            : base(brand, color, year, maxSpeed, wheelCount) 
        { 
            this.isSport = isSport;
        }

        // moving method for motorcycle 
        public override void Move()
        {
            Console.WriteLine($"Мотоцикл едет\n");
        }
    }

    public class Bicycle : LandVehicle
    {
        [BsonElement("HasHandBrakers")]
        public bool HasHandBrakers;

        // constructor for deserialization
        public Bicycle() { }

        // constructor for creating  bicycle objects
        public Bicycle(string brand, string color, int year, int maxSpeed, int wheelCount, bool handBrakers)
            : base(brand, color, year, maxSpeed, wheelCount)
        {
            HasHandBrakers = handBrakers;
        }

        // moving method for bicycle
        public override void Move()
        {
            Console.WriteLine($"Велосипед едет\n");
        }
    }
}
