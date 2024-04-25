using System;
using System.Collections.Generic;

namespace VehicleController.Models
{
    public partial class Make
    {
        public Make()
        {
            Cars = new HashSet<Car>();
            Models = new HashSet<Model>();
        }

        public int Id { get; set; }
        public string Name { get; set; } = null!;

        public virtual ICollection<Car> Cars { get; set; }
        public virtual ICollection<Model> Models { get; set; }
    }
}
