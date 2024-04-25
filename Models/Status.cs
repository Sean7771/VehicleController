using System;
using System.Collections.Generic;

namespace VehicleController.Models
{
    public partial class Status
    {
        public Status()
        {
            Vehicles = new HashSet<Vehicle>();
        }

        public int Id { get; set; }
        public string Name { get; set; } = null!;

        public virtual ICollection<Vehicle> Vehicles { get; set; }
    }
}
