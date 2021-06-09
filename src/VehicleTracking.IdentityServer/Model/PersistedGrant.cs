using System;
using VehicleTracking.Model;

namespace VehicleTracking.IdentityServer.Model
{
    public class PersistedGrant : BaseModel
    {
        public string Key { get; set; }

        public string Type { get; set; }

        public string SubjectId { get; set; }

        public string ClientId { get; set; }

        public DateTime CreationTime { get; set; }

        public DateTime Expiration { get; set; }

        public string Data { get; set; }        
    }
}
