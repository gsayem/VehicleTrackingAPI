using Microsoft.AspNetCore.Identity;

namespace VehicleTracking.IdentityServer.Model
{
    public class UserPasswordOptions : PasswordOptions
    {
        public int PasswordExpirationInDay { get; set; }
        public int PasswordHistoryLimit { get; set; }
        public string DefaultPassword { get; set; }
        public bool IsValidatePasswordExpiration { get; set; }
    }
}
