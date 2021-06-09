using System.Collections.Generic;
using VehicleTracking.Model;

namespace VehicleTracking.IdentityServer.Model
{
    public class ApiScope : BaseModel
    {
        //public int Id { get; set; }
        public string Name { get; set; }
        public string DisplayName { get; set; }
        public string Description { get; set; }
        public bool Required { get; set; }
        public bool Emphasize { get; set; }
        public bool ShowInDiscoveryDocument { get; set; } = true;
        public virtual List<ApiScopeClaim> UserClaims { get; set; }

        public virtual ApiResource ApiResource { get; set; }
    }
}
