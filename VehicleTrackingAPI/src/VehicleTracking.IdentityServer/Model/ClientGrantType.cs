using VehicleTracking.Model;

namespace VehicleTracking.IdentityServer.Model
{
    public class ClientGrantType : BaseModel
    {
        public string GrantType { get; set; }

        public virtual Client Client { get; set; }
    }
}
