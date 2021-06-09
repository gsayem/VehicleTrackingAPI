using VehicleTracking.Model;

namespace VehicleTracking.IdentityServer.Model
{
    public class ClientIdPRestriction : BaseModel
    {
        public string Provider { get; set; }

        public virtual Client Client { get; set; }
    }
}
