using VehicleTracking.Model;

namespace VehicleTracking.IdentityServer.Model
{
    public class ClientCorsOrigin : BaseModel
    {
        public string Origin { get; set; }

        public virtual Client Client { get; set; }
    }
}
