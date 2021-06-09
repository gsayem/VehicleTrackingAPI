using VehicleTracking.Model;

namespace VehicleTracking.IdentityServer.Model
{
    public class ClientSecret : Secret
    {
        public virtual Client Client { get; set; }
    }    
}
