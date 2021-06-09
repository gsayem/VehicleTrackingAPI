using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VehicleTracking.Model {
    public class Vehicle : BaseModel {
        public string Name { set; get; }
        public string RegistrationNumber { set; get; }        
        public string AspNetUserId { get; set; } // 

        public virtual ICollection<VehiclePosition> VehiclePositions { set; get; }
    }

    public class VehiclePosition : BaseModel {
        public double Latitude { set; get; }
        public double Longitude { set; get; }

        public Guid Vehicle_Id { set; get; }
        [ForeignKey("Vehicle_Id")]
        public virtual Vehicle Vehicle { set; get; }
    }
}
