using IdentityServer4.Configuration;
using IdentityServer4.Models;
using IdentityServer4.Services;
using IdentityServer4.Stores;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace VehicleTracking.IdentityServer.Services {
    public class TokenService : DefaultTokenService, ITokenService
    {
        public TokenService(IClaimsService claimsProvider, IReferenceTokenStore referenceTokenStore,
        ITokenCreationService creationService, IHttpContextAccessor contextAccessor, ISystemClock clock,
        IKeyMaterialService keyMaterialService, IdentityServerOptions options, ILogger<DefaultTokenService> logger)
            : base(claimsProvider, referenceTokenStore, creationService, contextAccessor, clock, keyMaterialService, options, logger)
        {

        }

        public override Task<Token> CreateIdentityTokenAsync(TokenCreationRequest request)
        {
            var localToken = base.CreateIdentityTokenAsync(request);
            return localToken;
        }

        public override Task<Token> CreateAccessTokenAsync(TokenCreationRequest request)
        {
            var localToken = base.CreateAccessTokenAsync(request);
            return localToken;
        }

        public override Task<string> CreateSecurityTokenAsync(Token token)
        {
            var localString = base.CreateSecurityTokenAsync(token);
            return localString;
        }

    }
}
