using VehicleTracking.Model;

namespace VehicleTracking.IdentityServer.Model
{
  public class ClientRedirectUri : BaseModel
    {
        public string RedirectUri { get; set; }

        public virtual Client Client { get; set; }
    }
}
