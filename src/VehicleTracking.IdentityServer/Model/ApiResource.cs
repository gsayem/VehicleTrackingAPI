using System.Collections.Generic;
using VehicleTracking.Model;

namespace VehicleTracking.IdentityServer.Model
{
    public class ApiResource : BaseModel
    {
        //public int Id { get; set; }
        public bool Enabled { get; set; } = true;
        public string Name { get; set; }
        public string DisplayName { get; set; }
        public string Description { get; set; }
        public virtual List<ApiSecret> Secrets { get; set; }
        public virtual List<ApiScope> Scopes { get; set; }
        public virtual List<ApiResourceClaim> UserClaims { get; set; }
    }
}
