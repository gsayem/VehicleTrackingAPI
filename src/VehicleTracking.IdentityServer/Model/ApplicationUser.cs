using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;

namespace VehicleTracking.IdentityServer.Model {
    // Add profile data for application users by adding properties to the ApplicationUser class
    public class ApplicationUser : IdentityUser<Guid> {
        public Guid ClientId { get; set; }
        public bool ForceResetPassword { get; set; }
        public DateTime PasswordExpireTime { get; set; }
        public virtual ICollection<PasswordHistory> PasswordHistories { get; set; }
    }


    public class ApplicationRole : IdentityRole<Guid> {
        //Add custom fields If need
    }
    public class ApplicationUserClaim : IdentityUserClaim<Guid> {
        //Add custom fields If need
    }
    public class ApplicationUserRole : IdentityUserRole<Guid> {
        //Add custom fields If need
    }
    public class ApplicationUserLogin : IdentityUserLogin<Guid> {
        //Add custom fields If need
    }
    public class ApplicationRoleClaim : IdentityRoleClaim<Guid> {
        //Add custom fields If need
    }
    public class ApplicationUserToken : IdentityUserToken<Guid> {
        //Add custom fields If need
    }
}
