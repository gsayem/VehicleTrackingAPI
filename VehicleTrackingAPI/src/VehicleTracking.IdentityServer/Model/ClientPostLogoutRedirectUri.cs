using VehicleTracking.Model;

namespace VehicleTracking.IdentityServer.Model
{
    public class ClientPostLogoutRedirectUri : BaseModel
    {
        public string PostLogoutRedirectUri { get; set; }

        public virtual Client Client { get; set; }
    }
}
