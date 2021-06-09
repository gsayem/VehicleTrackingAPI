using System.Collections.Generic;
using VehicleTracking.Model;

namespace VehicleTracking.IdentityServer.Model
{
    public class IdentityResource : BaseModel
    {
        public bool Enabled { get; set; } = true;
        public string Name { get; set; }
        public string DisplayName { get; set; }
        public string Description { get; set; }
        public bool Required { get; set; }
        public bool Emphasize { get; set; }
        public bool ShowInDiscoveryDocument { get; set; } = true;
        public virtual List<IdentityClaim> UserClaims { get; set; }
    }
}
