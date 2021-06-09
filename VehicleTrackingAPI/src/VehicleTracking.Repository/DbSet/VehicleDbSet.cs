using VehicleTracking.Model;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace VehicleTracking.Repository {
    public partial class VehicleTrackingDBContext {
        public DbSet<Vehicle> Vehicles { set; get; }
        public DbSet<VehiclePosition> VehiclePositions { set; get; }

        
    }
}
