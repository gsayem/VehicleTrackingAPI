namespace VehicleTracking.IdentityServer.Model
{
    public class ApiResourceClaim : UserClaim
    {
        public virtual ApiResource ApiResource { get; set; }
    }
}
