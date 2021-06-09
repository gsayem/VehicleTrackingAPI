using Microsoft.AspNetCore.Authorization;

namespace VehicleTracking.WebAPI.AuthorizationHandlers {
    public class AppTypeRequirement : IAuthorizationRequirement {
        public string ApplicationType { get; }

        public AppTypeRequirement(string applicationType) {
            ApplicationType = applicationType;
        }
    }
}
