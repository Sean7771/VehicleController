using System;
using System.Collections.Generic;

namespace VehicleController.Models
{
    public partial class Car
    {
        public int Id { get; set; }
        public int? VehicleId { get; set; }
        public int? MakeId { get; set; }
        public int? ModelId { get; set; }
        public int NumberOfDoors { get; set; }
        public string Color { get; set; } = null!;

        public virtual Make? Make { get; set; }
        public virtual Model? Model { get; set; }
        public virtual Vehicle? Vehicle { get; set; }
    }
}
