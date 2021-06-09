namespace VehicleTracking.IdentityServer.Model
{
    public class ApiScopeClaim : UserClaim
    {
        public virtual ApiScope ApiScope { get; set; }
    }
}
