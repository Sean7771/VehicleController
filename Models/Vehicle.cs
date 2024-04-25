using System;
using System.Collections.Generic;

namespace VehicleController.Models
{
    public partial class Vehicle
    {
        public Vehicle()
        {
            Cars = new HashSet<Car>();
            Trips = new HashSet<Trip>();
            Trucks = new HashSet<Truck>();
        }

        public int Id { get; set; }
        public string LicensePlate { get; set; } = null!;
        public decimal AverageSpeed { get; set; }
        public int? StatusId { get; set; }
        public decimal DistanceDriven { get; set; }
        public decimal DistanceReversed { get; set; }

        public virtual Status? Status { get; set; }
        public virtual ICollection<Car> Cars { get; set; }
        public virtual ICollection<Trip> Trips { get; set; }
        public virtual ICollection<Truck> Trucks { get; set; }
    }
}
