namespace VehicleTracking.IdentityServer.Model
{
    public class IdentityClaim : UserClaim
    {
        public virtual IdentityResource IdentityResource { get; set; }
    }
}
