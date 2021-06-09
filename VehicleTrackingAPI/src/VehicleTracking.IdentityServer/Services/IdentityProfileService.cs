using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using IdentityModel;
using IdentityServer4;
using IdentityServer4.AspNetIdentity;
using IdentityServer4.Extensions;
using IdentityServer4.Models;
using IdentityServer4.Services;
using Microsoft.AspNetCore.Identity;
using VehicleTracking.IdentityServer.Model;

namespace VehicleTracking.IdentityServer.Services {
    public class IdentityProfileService : ProfileService<ApplicationUser>, IProfileService {
        private readonly IUserClaimsPrincipalFactory<ApplicationUser> _claimsFactory;
        private readonly UserManager<ApplicationUser> _userManager;

        public IdentityProfileService(UserManager<ApplicationUser> userManager, IUserClaimsPrincipalFactory<ApplicationUser> claimsFactory) : base(userManager, claimsFactory) {
            _userManager = userManager;
            _claimsFactory = claimsFactory;
        }

        public override async Task GetProfileDataAsync(ProfileDataRequestContext context) {
            await base.GetProfileDataAsync(context);

            var sub = context.Subject.GetSubjectId();

            var user = await _userManager.FindByIdAsync(sub);
            var principal = await _claimsFactory.CreateAsync(user);

            var claims = principal.Claims.ToList();

            claims = claims.Where(claim => context.RequestedClaimTypes.Contains(claim.Type)).ToList();

            claims.Add(new Claim(JwtClaimTypes.GivenName, user.UserName));

            if (!claims.Exists(x => x.Type == "userId")) {
                claims.Add(new Claim("userId", (user as ApplicationUser)?.Id.ToString()));
            }

            var usersClaims = await _userManager.GetClaimsAsync(user);
            foreach (var claim in usersClaims) {
                claims.Add(claim);
            }

            claims.Add(new Claim(IdentityServerConstants.StandardScopes.Email, user.Email));
            //claims.Add(new Claim(IdentityServerConstants.AccessTokenAudience, "VehicleTrackingAPI"));
            //claims.Add(new Claim(JwtClaimTypes.Audience, 
            //    string.Format(accessTokenAudience, 
            //    context.HttpContext.GetIdentityServerIssuerUri().EnsureTrailingSlash())));

            context.IssuedClaims = claims;
        }

        public override async Task IsActiveAsync(IsActiveContext context) {
            var sub = context.Subject.GetSubjectId();
            var user = await _userManager.FindByIdAsync(sub);
            bool IsActive = false;
            if (user != null) {
                //if (user.EmailConfirmed && !user.ForceResetPassword && !user.LockoutEnabled)
                {
                    IsActive = true;
                }
            }

            context.IsActive = IsActive;
        }
    }
}
