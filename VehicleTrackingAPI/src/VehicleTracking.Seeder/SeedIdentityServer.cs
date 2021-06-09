using Microsoft.AspNetCore.Identity;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using VehicleTracking.Common.Extension;
using VehicleTracking.IdentityServer.Model;
using VehicleTracking.Interfaces.Repository;
using VehicleTracking.Repository;
using VehicleTracking.Web.Common.ViewModels;

namespace VehicleTracking.Seeder {
    public class SeedIdentityServer : SeedBase {

        public SeedIdentityServer() {
            

        }
        public void IdServerSetup() {
            var IdentityResourceRepo = new Repository<IdentityResource>(_identityServerDBContext);
            var ApiResourceRepo = new Repository<ApiResource>(_identityServerDBContext);

            var file = File.OpenRead(Path.Combine(Utils.GetProjectPath("VehicleTracking.Seeder"), "IdServerSetup.json"));
            var reader = new StreamReader(file);
            var jsonData = reader.ReadToEnd();
            dynamic json = JObject.Parse(jsonData);

            try {

                foreach (var item in json.IdentityResources) {
                    var userClaims1 = new List<IdentityClaim>();
                    foreach (var item1 in item.IdentityClaims) {
                        userClaims1.Add(new IdentityClaim { Type = item1.Type.ToString() });
                    }
                    var ff = new IdentityResource {
                        Description = item.Description.ToString(),
                        DisplayName = item.DisplayName.ToString(),
                        Emphasize = (bool)item.Emphasize,
                        Enabled = (bool)item.Enabled,
                        Name = item.Name.ToString(),
                        Required = (bool)item.Required,
                        ShowInDiscoveryDocument = (bool)item.ShowInDiscoveryDocument,
                        UserClaims = userClaims1
                    };

                    IdentityResourceRepo.Add(ff);
                }

                var userClaims = new List<ApiScopeClaim>();
                foreach (var item in json.ApiResources.ApiScopes.ApiScopeClaims) {
                    userClaims.Add(new ApiScopeClaim { Type = item.Type.ToString() });
                }

                ApiResource apiResource = new ApiResource {
                    Description = json.ApiResources.Description.ToString(),
                    DisplayName = json.ApiResources.DisplayName.ToString(),
                    Enabled = (bool)json.ApiResources.Enabled,
                    Name = json.ApiResources.Name.ToString(),
                    UserClaims = new List<ApiResourceClaim>() { new ApiResourceClaim { Type = json.ApiResources.ApiResourceClaims.Type.ToString() } },
                    Scopes = new List<ApiScope>() { new ApiScope {
                        Description = json.ApiResources.ApiScopes.Description.ToString(),
                        DisplayName = json.ApiResources.ApiScopes.DisplayName.ToString(),
                        Emphasize = (bool)json.ApiResources.ApiScopes.Emphasize,
                        Name = json.ApiResources.ApiScopes.Name.ToString(),
                        Required = (bool)json.ApiResources.ApiScopes.Required,
                        ShowInDiscoveryDocument = (bool)json.ApiResources.ApiScopes.ShowInDiscoveryDocument,
                        UserClaims = userClaims
                    } }
                };
                ApiResourceRepo.Add(apiResource);
            }
            catch (Exception ex) {
                throw ex;
            }

        }
    }
}
