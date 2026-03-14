using MongoDB.Bson.Serialization.Attributes;

namespace lab_3
{
    // determine airVehicle types for bson serialization
    [BsonKnownTypes(typeof(Airplane))]
    public class AirVehicle : Vehicle
    {
        [BsonElement("HeightOfFlight")]
        public int HeightOfFlight;

        // constructor for deserialization
        public AirVehicle() { }

        // constructor for creating airVehicle objects
        public AirVehicle(string brand, string color, int year, int maxSpeed, int flightHeight)
            : base(brand, color, year, maxSpeed) 
        { 
            HeightOfFlight = flightHeight;
        }

        // method for airVehicle moving - they fly
        public override void Move()
        {
            Console.WriteLine($"Лечу над облаками\n");
        }
    }

    public class Airplane : AirVehicle
    {
        [BsonElement("MotorsCount")]
        public int MotorsCount;

        // constructor for deserialization
        public Airplane() { }
        // constructor for creating airplane objects
        public Airplane(string brand, string color, int year, int maxSpeed, int height, int countMotors)
            : base(brand, color, year, maxSpeed, height) 
        { 
            MotorsCount = countMotors;
        }

        // method for airplane moving
        public override void Move()
        {
            Console.WriteLine($"Самолет летит\n");
        }
    }
}
