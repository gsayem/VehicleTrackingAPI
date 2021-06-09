using AutoMapper;
using IdentityServer4.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VehicleTracking.Common.Extension;
using VehicleTracking.IdentityServer.Model;
using VehicleTracking.IdentityServer.Services;
using VehicleTracking.IdentityServer.Web.Configuration;
using VehicleTracking.IdentityServer.Web.ViewModels;
using VehicleTracking.Web.Common.ViewModels;
using Client = VehicleTracking.IdentityServer.Model.Client;

namespace VehicleTracking.IdentityServer.Web.API {
    [Produces("application/json")]
    [ApiController]
    public class ClientController : ControllerBase {
        private readonly IClientService _clientService;
        private IMapper _mapper;
        public ClientController(IClientService clientService, IMapper mapper) {
            _clientService = clientService;
            _mapper = mapper;
        }

        [HttpPost]
        [ProducesResponseType(typeof(ResponseMessage<string>), 200)]
        [Route("api/clients")]
        public async Task<IActionResult> PostAsync([FromBody] ClientRequestViewModel clientRequestViewModel,
            [FromServices] IOptionsMonitor<IdServerClientConfiguration> idServerClientConfiguration) {
            if (clientRequestViewModel == null || string.IsNullOrWhiteSpace(clientRequestViewModel.ClientId)) {
                return BadRequest(new ResponseMessage<string>(Status.FAILURE, Code.DATA_NOT_PROVIDED, "Please provide id to create client."));
            }

            var clientScope = _mapper.Map<IEnumerable<ClientScopeViewModel>, IEnumerable<ClientScope>>(idServerClientConfiguration.CurrentValue.ClientScopes).ToList();
            var clientGrantTypes = _mapper.Map<IEnumerable<ClientGrantTypeViewModel>, IEnumerable<ClientGrantType>>(idServerClientConfiguration.CurrentValue.ClientGrantTypes).ToList();

            if (await _clientService.IsUniqueClient(clientRequestViewModel.ClientId)) {
                var client = new Client {
                    ClientId = clientRequestViewModel.ClientId,
                    ClientName = clientRequestViewModel.ClientName,
                    AllowedScopes = clientScope,
                    AllowedGrantTypes = clientGrantTypes
                };
                await _clientService.AddClient(client);
            } else {
                return BadRequest(new ResponseMessage<string>(Status.FAILURE, Code.DATA_ALREADY_EXIST, "Client id already exist."));
            }

            return Ok(new ResponseMessage<string>(Status.SUCCESS, Code.CREATE, "Client create successfully"));
        }
        [HttpGet]
        [ProducesResponseType(typeof(ResponseMessage<ClientViewModel>), 200)]
        [Route("api/clients/{clientId}")]
        public async Task<IActionResult> GetAsync(string clientId) {
            var client = await _clientService.FindByClientIdAsync(clientId);
            if (client != null) {
                ClientViewModel clientViewModel = _mapper.Map<Client, ClientViewModel>(client);
                return Ok(new ResponseMessage<ClientViewModel>(Status.SUCCESS, Code.GET, "Data get successfully", clientViewModel));
            } else {
                return NotFound(new ResponseMessage<string>(Status.FAILURE, Code.DATA_NOT_FOUND, "Client not found."));
            }
        }
        [HttpPut]
        [ProducesResponseType(201)]
        [ProducesResponseType(400)]
        [Route("api/clients/{clientId}/generatesecret")]
        [AllowAnonymous]
        public async Task<IActionResult> GenerateSecret(string clientId) {
            Client client = await _clientService.FindByClientIdAsync(clientId);
            if (client != null) {
                string secret = SecretGenerator.Generate();
                var userSecret = new UserSecretViewModel() {
                    CustomerSecret = secret,
                    Hash = secret.Sha256()
                };

                ClientSecret clientSecret = await _clientService.FindClientSecretByClient(client);
                if (clientSecret == null) {
                    clientSecret = new ClientSecret {
                        Client = client,
                        Description = "Secret Key",
                        Value = userSecret.Hash
                    };
                } else {
                    clientSecret.Value = userSecret.Hash;
                }
                await _clientService.SaveClientSecret(clientSecret);
                return Ok(new ResponseMessage<string>(Status.SUCCESS, Code.CREATE, "Secret create successfully.", userSecret.CustomerSecret));
            } else {
                return NotFound(new ResponseMessage<string>(Status.FAILURE, Code.DATA_NOT_FOUND, "Client not found."));
            }
        }
    }
}
