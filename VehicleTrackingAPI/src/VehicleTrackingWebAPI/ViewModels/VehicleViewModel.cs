using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace VehicleTracking.WebAPI.ViewModels {
    public class VehicleViewModel {
        public string Name { set; get; }
        public string RegistrationNumber { set; get; }
        /// <summary>
        /// It'll be user name
        /// </summary>
        public string Email { set; get; }
        public string Password { set; get; }
        public string ClientId { set; get; } = "SevenPeaks";
    }
    public class VehicleResponseViewModel {
        public string LoginUserId { set; get; }
    }

    public class VehiclePositionViewModel {
        /// <summary>
        /// It'll be user name
        /// </summary>
        public string Email { set; get; }
        public double Latitude { set; get; }
        public double Longitude { set; get; }
    }
}
