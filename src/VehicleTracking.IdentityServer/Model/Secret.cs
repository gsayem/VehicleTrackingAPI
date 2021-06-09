using System;
using VehicleTracking.Model;

namespace VehicleTracking.IdentityServer.Model
{
    public abstract class Secret : BaseModel
    {
        public string Type { get; set; } = "SharedSecret";
        
        public string Description { get; set; }

        public string Value { get; set; }

        public DateTime? Expiration { get; set; }
    }
}
