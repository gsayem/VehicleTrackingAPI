using VehicleTracking.Model;

namespace VehicleTracking.IdentityServer.Model
{
    public abstract class UserClaim: BaseModel
    {
        public string Type { get; set; }
    }
}
