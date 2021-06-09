using System;
using VehicleTracking.Model;

namespace VehicleTracking.IdentityServer.Model {
    public class PasswordHistory : BaseModel {
        public string PasswordHash { get; set; }
        public Guid UserId { get; set; }
        public virtual ApplicationUser User { get; set; }
    }
}

