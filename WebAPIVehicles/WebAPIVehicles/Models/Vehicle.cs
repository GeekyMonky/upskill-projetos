namespace WebAPIVehicles.Models
{
    public class Vehicle
    {
        public int VehicleID { get; set; }

        public string Brand { get; set; }

        public string Model { get; set; }

        public int Year { get; set; }

        public DateTime? LastInspection { get; set; }

        public bool Sold { get; set; }
    }
}