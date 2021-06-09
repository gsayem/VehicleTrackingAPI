using Microsoft.AspNetCore.Authorization;
using System.Linq;
using System.Threading.Tasks;
using static VehicleTracking.WebAPI.Infrastructure.ApiConstants;

namespace VehicleTracking.WebAPI.AuthorizationHandlers {
    public class CallingApplicationHandler : AuthorizationHandler<AppTypeRequirement> {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, AppTypeRequirement requirement) {
            if (context.User.HasClaim(c => c.Type == ClaimType.AppTypeKey) &&
                context.User.Claims.Any(c => c.Type == ClaimType.AppTypeKey && c.Value == requirement.ApplicationType)) {
                //Log.Info($"User has apprequirements: {requirement.ApplicationType}");
                context.Succeed(requirement);
            }
            else {
                //Log.Info($"User does not have apprequirements: {requirement.ApplicationType}");
            }

            return Task.CompletedTask;
        }
    }
}
