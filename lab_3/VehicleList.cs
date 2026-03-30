using MongoDB.Bson.Serialization.Attributes;
using System.Collections.Generic;

namespace lab_3
{
    // class for writing list of objects into BSON file
    public class VehicleList
    {
        [BsonElement("Vehicles")]
        public List<Vehicle> Vehicles { get; set; }

        public VehicleList()
        {
            Vehicles = new List<Vehicle>();
        }

        public VehicleList(List<Vehicle> vehicles)
        {
            Vehicles = vehicles;
        }
    }
}
