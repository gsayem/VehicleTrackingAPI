using IdentityServer4.Validation;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;
using VehicleTracking.IdentityServer.Model;

namespace VehicleTracking.IdentityServer {
    public class ResourceOwnerPasswordValidator : IResourceOwnerPasswordValidator
    {
        private readonly SignInManager<ApplicationUser> _signInManager;

        public ResourceOwnerPasswordValidator(SignInManager<ApplicationUser> signInManager)
        {
            _signInManager = signInManager;
        }

        public Task ValidateAsync(ResourceOwnerPasswordValidationContext context)
        {
            context.Result.IsError = false;

            return Task.FromResult(0);
        }
    }
}
