using System;
using System.Collections.Generic;

namespace VehicleController.Models
{
    public partial class Model
    {
        public Model()
        {
            Cars = new HashSet<Car>();
        }

        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public int? MakeId { get; set; }

        public virtual Make? Make { get; set; }
        public virtual ICollection<Car> Cars { get; set; }
    }
}
