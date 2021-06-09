using VehicleTracking.Model;

namespace VehicleTracking.IdentityServer.Model
{
    public class ClientClaim : BaseModel
    {
        public string Type { get; set; }

        public string Value { get; set; }

        public virtual Client Client { get; set; }
    }
}
