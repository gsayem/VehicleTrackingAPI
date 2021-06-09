using IdentityServer4.Services;
using IdentityServer4.Validation;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace VehicleTracking.IdentityServer.Services {
    public class ClaimsService : DefaultClaimsService, IClaimsService
    {
        public ClaimsService(IProfileService profile, ILogger<DefaultClaimsService> logger) : base(profile, logger)
        {
        }

        public override Task<IEnumerable<Claim>> GetIdentityTokenClaimsAsync(ClaimsPrincipal subject, ResourceValidationResult resources,
            bool includeAllIdentityClaims, ValidatedRequest request)
        {
            var myIncludethem = true;
            var localResult = base.GetIdentityTokenClaimsAsync(subject, resources, myIncludethem, request);
            return localResult;
        }
        public override Task<IEnumerable<Claim>> GetAccessTokenClaimsAsync(ClaimsPrincipal subject, ResourceValidationResult resources, ValidatedRequest request)
        {
            var localResult = base.GetAccessTokenClaimsAsync(subject, resources, request);
            return localResult;
        }
    }
}
