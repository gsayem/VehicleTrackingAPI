using System;
using System.Collections.Generic;

namespace VehicleTracking.Web.Common.ViewModels {
    public class UserViewModel {
        public Guid ClientId { get; set; }
        public Guid UserId { get; set; }
        public string UserName { get; set; }
        public string PasswordHash { get; set; }
        public string EmailAddress { get; set; }
        public bool IsActive { get; set; }
        public bool IsAdmin { get; set; }
        public bool ForceResetPassword { get; set; }
        public ICollection<UserClaimViewModel> Claims { set; get; }
    }
    public class UserClaimViewModel {
        public string Type { get; set; }
        public string Value { get; set; }
    }
}
