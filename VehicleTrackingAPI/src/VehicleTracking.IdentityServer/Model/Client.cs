using System;
using System.Collections.Generic;
using VehicleTracking.Model;


namespace VehicleTracking.IdentityServer.Model
{
    public class Client : BaseModel
    {
        public bool Enabled { get; set; } = true;
        public string ProtocolType { get; set; } = "oidc";
        public bool RequireClientSecret { get; set; } = true;
        public bool RequireConsent { get; set; } = false;
        public bool AllowRememberConsent { get; set; } = true;
        public bool LogoutSessionRequired { get; set; } = true;
        public int IdentityTokenLifetime { get; set; } = 300;
        public int AccessTokenLifetime { get; set; } = 3600; // In Second
        public int AuthorizationCodeLifetime { get; set; } = 300;
        public int AbsoluteRefreshTokenLifetime { get; set; } = 2592000;
        public int SlidingRefreshTokenLifetime { get; set; } = 1296000;
        public int RefreshTokenUsage { get; set; } = 1;
        public int RefreshTokenExpiration { get; set; } = 1;
        public bool EnableLocalLogin { get; set; } = true;
        public bool PrefixClientClaims { get; set; } = true;
        public string ClientId { get; set; }
        public virtual ICollection<ClientSecret> ClientSecrets { get; set; }
        public string ClientName { get; set; }
        public string ClientUri { get; set; }
        public string LogoUri { get; set; }
        public virtual ICollection<ClientGrantType> AllowedGrantTypes { get; set; }
        public bool RequirePkce { get; set; } = false;
        public bool AllowPlainTextPkce { get; set; } = false;
        public bool AllowAccessTokensViaBrowser { get; set; } = true;
        public virtual ICollection<ClientRedirectUri> RedirectUris { get; set; }
        public virtual ICollection<ClientPostLogoutRedirectUri> PostLogoutRedirectUris { get; set; }
        public string LogoutUri { get; set; }
        public bool AllowOfflineAccess { get; set; } = true;
        public virtual ICollection<ClientScope> AllowedScopes { get; set; }
        public bool UpdateAccessTokenClaimsOnRefresh { get; set; } = false;
        public int AccessTokenType { get; set; } = 0;
        public virtual ICollection<ClientIdPRestriction> IdentityProviderRestrictions { get; set; }
        public bool IncludeJwtId { get; set; } = false;
        public virtual ICollection<ClientClaim> Claims { get; set; }
        public bool AlwaysSendClientClaims { get; set; } = true;
        public virtual ICollection<ClientCorsOrigin> AllowedCorsOrigins { get; set; }
    }
}
