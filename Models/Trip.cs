using System;
using System.Collections.Generic;

namespace VehicleController.Models
{
    public partial class Trip
    {
        public int Id { get; set; }
        public int? VehicleId { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime? EndTime { get; set; }
        public decimal Distance { get; set; }
        
        public virtual Vehicle? Vehicle { get; set; }
    }
}
