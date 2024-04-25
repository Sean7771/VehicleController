using System;
using System.Collections.Generic;

namespace VehicleController.Models
{
    public partial class Truck
    {
        public int Id { get; set; }
        public int? VehicleId { get; set; }
        public int CargoCapacity { get; set; }
        public bool HasTrailer { get; set; }

        public virtual Vehicle? Vehicle { get; set; }
    }
}
