using System.Collections.Generic;
using VehicleTracking.IdentityServer.Web.ViewModels;

namespace VehicleTracking.IdentityServer.Web.Configuration {
    public class IdServerClientConfiguration {
        public List<ClientScopeViewModel> ClientScopes { get; set; }
        public List<ClientGrantTypeViewModel> ClientGrantTypes { get; set; }
    }
}
