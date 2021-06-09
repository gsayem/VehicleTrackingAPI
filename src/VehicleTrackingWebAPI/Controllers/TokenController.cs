using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using VehicleTracking.Common.Extension;
using VehicleTracking.Interfaces.Services;
using VehicleTracking.Model;
using VehicleTracking.Web.Common.ViewModels;
using VehicleTracking.WebAPI.AuthorizationHandlers;
using VehicleTracking.WebAPI.ViewModels;
using static VehicleTracking.WebAPI.Infrastructure.ApiConstants;

namespace VehicleTracking.WebAPI.Controllers {

    /// <summary>
    /// Token Controller only for swagger
    /// </summary>
    [ApiController]
    [Route("[controller]")]
    public class TokenController : ControllerBase {
        private readonly ILogger<TokenController> _logger;
        public TokenController(ILogger<TokenController> logger) {
            _logger = logger;
        }

        [HttpPost]
        [ProducesResponseType(typeof(string), 200)]
        [ProducesResponseType(typeof(ResponseMessage<string>), 400)]
        [ProducesResponseType(typeof(ResponseMessage<string>), 401)]
        [ProducesResponseType(typeof(ResponseMessage<string>), 404)]
        [AllowAnonymous]
        public async Task<IActionResult> GetBearerToken(TokenViewModel tokenViewModel) {
            var _idServerUrl = "http://localhost:5000"; //As there is no UI and it's only for swagger and test.
            var AccessToken = string.Empty;
            var keyValues = new[]{
                new KeyValuePair<string, string>("client_id",  tokenViewModel.ClientId),
                new KeyValuePair<string, string>("client_secret", tokenViewModel.ClientSecret),
                new KeyValuePair<string, string>("username", tokenViewModel.UserName),
                new KeyValuePair<string, string>("password", tokenViewModel.Password),
                new KeyValuePair<string, string>("grant_type", "password"),
                new KeyValuePair<string, string>("response_type", "token"),
                new KeyValuePair<string, string>("scope", "VehicleTrackingAPI")
           };
            var formBody = new FormUrlEncodedContent(keyValues);
            using (var httpClient = new HttpClient()) {
                httpClient.Timeout = TimeSpan.FromMinutes(10); //For test and debug
                var postRequest = new HttpRequestMessage(HttpMethod.Post, new Uri(_idServerUrl + "/connect/token")) {
                    Content = formBody
                };

                var result = await httpClient.SendAsync(postRequest);
                //result.EnsureSuccessStatusCode();
                var returnString = await result.Content.ReadAsStringAsync();
                AccessToken = JsonConvert.DeserializeObject(returnString).ToString();
            }
            return Ok(AccessToken);
        }
    }
}
