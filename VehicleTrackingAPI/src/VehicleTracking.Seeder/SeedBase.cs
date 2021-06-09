using Microsoft.AspNetCore.Identity;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using VehicleTracking.Common.Extension;
using VehicleTracking.IdentityServer.Model;
using VehicleTracking.IdentityServer.Repository;
using VehicleTracking.Interfaces.Repository;
using VehicleTracking.Repository;
using VehicleTracking.Web.Common.ViewModels;

namespace VehicleTracking.Seeder {
    public abstract class SeedBase {
        protected string _idServerUrl;
        protected string _webApiUrl;
        protected VehicleTrackingDBContext _vehicleTrackingDBContext;
        protected IdentityServerDBContext _identityServerDBContext;
        private static string _accessToken;
        protected HttpProxy _httpProxy;

        protected IRepository<Client> clientRepo;
        protected IRepository<ClientRedirectUri> clientRedirectUriRepo;
        protected IRepository<ClientPostLogoutRedirectUri> clientPostLogoutRedirectUriRepo;
        protected IRepository<ClientCorsOrigin> clientCorsOriginsRepo;
        protected IRepository<ClientScope> clientScopesRepo;
        protected IRepository<ClientGrantType> clientGrantTypeRepo;
        protected IRepository<ClientSecret> clientSecretRepo;

        public string AccessToken {
            get => _accessToken;
            private set {
                if (string.IsNullOrWhiteSpace(_accessToken) || _accessToken != value) {
                    _accessToken = value;
                    _httpProxy = new HttpProxy(value);
                }

            }
        }
        public SeedBase() {
            _httpProxy = new HttpProxy();
            _idServerUrl = Environment.GetEnvironmentVariable(EnvironmentVariableKeys.IdentityUrl);
            _webApiUrl = Environment.GetEnvironmentVariable(EnvironmentVariableKeys.ApiUrl);
            _vehicleTrackingDBContext = new VehicleTrackingDBContext(Environment.GetEnvironmentVariable(EnvironmentVariableKeys.ApiSqlConnection));
            _identityServerDBContext = new IdentityServerDBContext(Environment.GetEnvironmentVariable(EnvironmentVariableKeys.IdentitySqlConnection));

            clientRepo = new Repository<Client>(_identityServerDBContext);
            clientRedirectUriRepo = new Repository<ClientRedirectUri>(_identityServerDBContext);
            clientPostLogoutRedirectUriRepo = new Repository<ClientPostLogoutRedirectUri>(_identityServerDBContext);
            clientCorsOriginsRepo = new Repository<ClientCorsOrigin>(_identityServerDBContext);
            clientScopesRepo = new Repository<ClientScope>(_identityServerDBContext);
            clientGrantTypeRepo = new Repository<ClientGrantType>(_identityServerDBContext);
            clientSecretRepo = new Repository<ClientSecret>(_identityServerDBContext);
        }



        #region Clients
        protected async Task<string> GenerateClientSecretAsync(string ClientId) {
            string clientSecret = string.Empty;
            using (var httpCLient = new HttpClient()) {
                var clientUrl = $"{_idServerUrl}/api/clients/{ClientId}/generatesecret";
                var result = httpCLient.PutAsync(clientUrl, null).Result;
                if (result.IsSuccessStatusCode) {
                    var json = await result.Content.ReadAsStringAsync();
                    var response = JsonConvert.DeserializeObject<ResponseMessage<string>>(json);
                    clientSecret = response.Data;
                }
            }
            return clientSecret;
        }

        protected async Task CreateClientAsync(string ClientId, string ClientName) {
            using (var httpCLient = new HttpClient()) {
                var clientUrl = $"{_idServerUrl}/api/clients";
                var data = new { ClientId, ClientName };
                var result = await httpCLient.PostAsync(clientUrl, new StringContent(JsonConvert.SerializeObject(data), Encoding.UTF8, "application/json"));
                if (result.IsSuccessStatusCode) {
                    Log.Info($"Client created  {ClientName}");
                } else {
                    Log.Error($"Client did not created  {ClientName}");
                }
            }
        }
        protected async Task SaveClientSecret(string ClientId, string ClientsSecretAfterSHA256) {
            if (!ClientsSecretAfterSHA256.IsNullOrWhiteSpace()) {
                Client client = clientRepo.Find(cl => cl.ClientId == ClientId);
                if (client != null) {
                    ClientSecret clientSecret = clientSecretRepo.Find(cl => cl.Client.Id == client.Id);
                    if (clientSecret == null) {
                        clientSecret = new ClientSecret {
                            Client = client,
                            Description = "Secret Key",
                            Value = ClientsSecretAfterSHA256
                        };
                    } else {
                        clientSecret.Value = ClientsSecretAfterSHA256;
                    }

                    if (clientSecret.Id.IsNotNull()) {
                        clientSecretRepo.Update(clientSecret);
                    } else {
                        clientSecretRepo.Add(clientSecret);
                    }
                }
            }
        }

        protected async Task<Client> CreateClientRelatedData(string clientId, dynamic jsonDefaultData) {
            var client = clientRepo.Find(c => c.ClientId == clientId);
            if (jsonDefaultData != null) {
                if (jsonDefaultData.ClientRedirectUris != null) {
                    foreach (var clientRedirectUri in jsonDefaultData.ClientRedirectUris) {
                         clientRedirectUriRepo.Add(new ClientRedirectUri { Client = client, RedirectUri = clientRedirectUri.RedirectUri.Value });
                    }
                }
                if (jsonDefaultData.ClientPostLogoutRedirectUris != null) {
                    foreach (var clientPostLogoutRedirectUri in jsonDefaultData.ClientPostLogoutRedirectUris) {
                         clientPostLogoutRedirectUriRepo.Add(new ClientPostLogoutRedirectUri { Client = client, PostLogoutRedirectUri = clientPostLogoutRedirectUri.PostLogoutRedirectUri.Value });
                    }
                }
                if (jsonDefaultData.ClientCorsOrigins != null) {
                    foreach (var clientCorsOrigins in jsonDefaultData.ClientCorsOrigins) {
                        clientCorsOriginsRepo.Add(new ClientCorsOrigin { Client = client, Origin = clientCorsOrigins.Origin.Value });
                    }
                }
                if (jsonDefaultData.ClientScopes != null && jsonDefaultData.ClientScopes.Count > 0) {
                    client.AllowedScopes.Reverse().ForEach(g => clientScopesRepo.Remove(g));
                    foreach (var clientScope in jsonDefaultData.ClientScopes) {
                        clientScopesRepo.Add(new ClientScope { Client = client, Scope = clientScope.Scope.Value });
                    }
                }
                if (jsonDefaultData.ClientGrantTypes != null && jsonDefaultData.ClientGrantTypes.Count > 0) {
                    client.AllowedGrantTypes.Reverse().ForEach(g => clientGrantTypeRepo.Remove(g));
                    foreach (var clientGrantTypes in jsonDefaultData.ClientGrantTypes) {
                        clientGrantTypeRepo.Add(new ClientGrantType { Client = client, GrantType = clientGrantTypes.GrantType.Value });
                    }
                }
            }
            return client;
        }
        #endregion

        #region User
        protected async Task<string> CreateUserAsync(string emailAddress, string password, Guid clientId, IList<UserClaimViewModel> claims) {
            var user = new UserViewModel {
                ClientId = clientId,
                UserName = emailAddress,
                EmailAddress = emailAddress,
                PasswordHash = password,
                Claims = claims
            };
            string userId = null;
            using (var httpCLient = new HttpClient()) {
                var jsonData = JsonConvert.SerializeObject(user);
                var userUrl = _idServerUrl + "/api/users";
                var result = httpCLient.PostAsync(userUrl, new StringContent(jsonData, Encoding.UTF8, "application/json")).Result;
                if (result.IsSuccessStatusCode) {
                    dynamic dn = JsonConvert.DeserializeObject(await result.Content.ReadAsStringAsync());
                    userId = dn.id;
                    var userUnLockUrl = _idServerUrl + "/api/users/" + emailAddress + "/unlock";
                    result = await httpCLient.PutAsync(userUnLockUrl, new StringContent(string.Empty));
                } else {
                    string ds = await result.Content.ReadAsStringAsync();
                    List<IdentityError> errors = JsonConvert.DeserializeObject<List<IdentityError>>(ds);
                    if (errors != null & errors.Count > 0) {
                        Log.Error($"User can't create for {user.EmailAddress}, {errors[0].Description}");
                    }
                }
            }

            return userId;
        }
        protected List<UserClaimViewModel> PrepareUserClaims(dynamic jsonData) {
            var claims = new List<UserClaimViewModel>();
            try {
                if (jsonData.User.Claims != null) {
                    foreach (var claim in jsonData.User.Claims) {
                        claims.Add(new UserClaimViewModel() { Type = claim.Type, Value = claim.Value });
                    }
                }
            } catch { }
            return claims;
        }
        #endregion
        protected async Task SetBearerTokenAsync(string clientId, string clientSecretCode, string username, string password) {
            using (var httpClient = new HttpClient()) {
                var formBody = Utils.GetFormEncodedContent(clientId, clientSecretCode, username, password);

                var postRequest = new HttpRequestMessage(HttpMethod.Post, new Uri(_idServerUrl + "/connect/token")) {
                    Content = formBody
                };

                var result = await httpClient.SendAsync(postRequest);
                result.EnsureSuccessStatusCode();
                var returnString = await result.Content.ReadAsStringAsync();
                dynamic json = JObject.Parse(JsonConvert.DeserializeObject(returnString).ToString());
                AccessToken = json.access_token;
            }
        }
    }
}
