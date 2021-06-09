using IdentityServer4.Extensions;
using IdentityServer4.ResponseHandling;
using IdentityServer4.Services;
using IdentityServer4.Validation;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using VehicleTracking.IdentityServer.Model;

namespace VehicleTracking.IdentityServer.Services {

    public class IdentityServerAuthorizeInteractionResponseGenerator : AuthorizeInteractionResponseGenerator, IAuthorizeInteractionResponseGenerator
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public IdentityServerAuthorizeInteractionResponseGenerator(UserManager<ApplicationUser> userManager, ILogger<AuthorizeInteractionResponseGenerator> logger,
            ISystemClock systemClock, IConsentService consent, IProfileService profile) : base(systemClock, logger, consent, profile)
        {
            _userManager = userManager;
        }

        protected override async Task<InteractionResponse> ProcessLoginAsync(ValidatedAuthorizeRequest request)
        {
            var baseInteractionResponseResult = await base.ProcessLoginAsync(request);

            if (baseInteractionResponseResult.IsLogin || baseInteractionResponseResult.IsError)
            {
                return baseInteractionResponseResult;
            }

            var subjectId = request.Subject.GetSubjectId();
            if (string.IsNullOrWhiteSpace(subjectId))
            {
                return new InteractionResponse();
            }

            ApplicationUser user = await _userManager.FindByIdAsync(subjectId);
            if (user == null)
            {
                return new InteractionResponse();
            }

            if (user.ForceResetPassword)
            {
                return new InteractionResponse() { IsLogin = true };
            }

            return baseInteractionResponseResult;
        }
    }
}
