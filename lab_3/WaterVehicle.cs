using MongoDB.Bson.Serialization.Attributes;

namespace lab_3
{
    // determine waterVehicle types for bson serialization
    [BsonKnownTypes(typeof(Boat), typeof(Liner))]
    public class WaterVehicle : Vehicle
    {
        [BsonElement("Size")]
        public string Size;

        // constructor for deserialization
        public WaterVehicle() { }

        // constructor for creating waterVehicle objects
        public WaterVehicle(string brand, string color, int year, int maxSpeed, string size)
            : base(brand, color, year, maxSpeed)
        {
            Size = size;
        }

        // method for land vehicles - they sail
        public override void Move()
        {
            Console.WriteLine("Плыву по воде\n");
        }
    }

    public class Boat : WaterVehicle
    {
        [BsonElement("Material")]
        public string Material;

        // constructor for deserialization
        public Boat() { }

        // constructor for creating boat objects
        public Boat(string brand, string color, int year, int maxSpeed, string size, string material)
            : base(brand, color, year, maxSpeed, size) 
        {
            Material = material;
        }

        // moving method for boats
        public override void Move()
        {
            Console.WriteLine("Лодка плывет\n");
        }
    }

    public class Liner : WaterVehicle
    {
        [BsonElement("FloorsCount")]
        public int FloorsCount;

        // constructor for deserialization
        public Liner() { }

        // constructor for creating liner objects
        public Liner(string brand, string color, int year, int maxSpeed, string size, int floors)
            : base(brand, color, year, maxSpeed, size)
        {  
            FloorsCount = floors;
        }

        // moving methods for moving
        public override void Move()
        {
            Console.WriteLine($"Лайнер плывет\n");
        }
    } 
}
