namespace VehicleTracking.IdentityServer.Model
{
    public class ApiSecret : Secret
    {
        public virtual ApiResource ApiResource { get; set; }
    }

}
