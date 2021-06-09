using System.Collections.Generic;

namespace VehicleTracking.IdentityServer.Web.ViewModels {
    public class ClientRequestViewModel
    {
        public string ClientId { get; set; }
        public string ClientName { get; set; }
    }

    public class ClientScopeViewModel {
        public string Scope { get; set; }

    }
    public class ClientGrantTypeViewModel {
        public string GrantType { get; set; }
        //public ClientViewModel Client { get; set; }
    }
    public class ClientViewModel {
        public string Id { get; set; }
        public bool Enabled { get; set; }
        public string ProtocolType { get; set; }
        public bool RequireClientSecret { get; set; }
        public bool RequireConsent { get; set; }
        public bool AllowRememberConsent { get; set; }
        public bool LogoutSessionRequired { get; set; }
        public int IdentityTokenLifetime { get; set; }
        public int AccessTokenLifetime { get; set; }
        public int AuthorizationCodeLifetime { get; set; }
        public int AbsoluteRefreshTokenLifetime { get; set; }
        public int SlidingRefreshTokenLifetime { get; set; }
        public int RefreshTokenUsage { get; set; }
        public int RefreshTokenExpiration { get; set; }
        public bool EnableLocalLogin { get; set; }
        public bool PrefixClientClaims { get; set; }
        public string ClientId { get; set; }
        public string ClientName { get; set; }
        public string ClientUri { get; set; }
        public string LogoUri { get; set; }
        public List<ClientGrantTypeViewModel> AllowedGrantTypes { get; set; }
        public bool RequirePkce { get; set; }
        public bool AllowPlainTextPkce { get; set; }
        public bool AllowAccessTokensViaBrowser { get; set; }
        public string LogoutUri { get; set; }
        public bool AllowOfflineAccess { get; set; }
        public List<ClientScopeViewModel> AllowedScopes { get; set; }
        public bool UpdateAccessTokenClaimsOnRefresh { get; set; }
        public int AccessTokenType { get; set; }
        public bool IncludeJwtId { get; set; }
        public bool AlwaysSendClientClaims { get; set; }
    }
    public class ClientRedirectUriViewModel {
        public string RedirectUri { get; set; }
    }
    public class ClientPostLogoutRedirectUriViewModel {
        public string PostLogoutRedirectUri { get; set; }
    }
}
