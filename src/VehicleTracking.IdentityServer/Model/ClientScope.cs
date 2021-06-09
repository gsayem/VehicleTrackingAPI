using VehicleTracking.Model;

namespace VehicleTracking.IdentityServer.Model
{
    public class ClientScope : BaseModel
    {
        public string Scope { get; set; }

        public virtual Client Client { get; set; }
    }
}
