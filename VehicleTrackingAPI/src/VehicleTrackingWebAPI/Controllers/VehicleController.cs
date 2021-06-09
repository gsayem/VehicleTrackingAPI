using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using VehicleTracking.Common.Extension;
using VehicleTracking.Interfaces.Services;
using VehicleTracking.Model;
using VehicleTracking.Web.Common.ViewModels;
using VehicleTracking.WebAPI.ViewModels;

namespace VehicleTracking.WebAPI.Controllers {
    [ApiController]
    [Route("[controller]")]
    public class VehicleController : ControllerBase {
        private readonly string _idServerUrl = "http://localhost:5000"; //As there is no UI and it's only for swagger and test.
        private readonly ILogger<VehicleController> _logger;

        public VehicleController(ILogger<VehicleController> logger) {
            _logger = logger;
        }



        [HttpPost]
        [ProducesResponseType(typeof(ResponseMessage<VehicleResponseViewModel>), 200)]
        [ProducesResponseType(typeof(ResponseMessage<string>), 400)]
        [ProducesResponseType(typeof(ResponseMessage<string>), 401)]
        [ProducesResponseType(typeof(ResponseMessage<string>), 404)]
        [AllowAnonymous]
        public async Task<IActionResult> VehicleRegistration(VehicleViewModel vehicleViewModel, [FromServices] IVehicleService vehicleService) {
            string message = string.Empty;
            Status status = Status.FAILURE;
            Code code = Code.CREATE;
            VehicleResponseViewModel vehicleResponseViewModel = new();
            if (vehicleViewModel == null) {
                message = "Vehicle data not found";
                code = Code.DATA_NOT_FOUND;
            }

            if (message.IsNullOrWhiteSpace() && vehicleViewModel.Name.IsNullOrWhiteSpace()) {
                message = "Name required";
                code = Code.DATA_NOT_FOUND;
            }
            if (message.IsNullOrWhiteSpace() && vehicleViewModel.RegistrationNumber.IsNullOrWhiteSpace()) {
                message = "Registration Number required";
                code = Code.DATA_NOT_FOUND;
            }
            string clientIdGuid = string.Empty;
            if (message.IsNullOrWhiteSpace()) {
                clientIdGuid = await GetClientAsync(vehicleViewModel.ClientId);
                if (clientIdGuid.IsNullOrWhiteSpace()) {
                    message = "Invalid client";
                    code = Code.DATA_NOT_FOUND;
                }
            }


            if (message.IsNullOrWhiteSpace()) {
                try {
                    if (await vehicleService.FindVehicleByRegistrationNumberAsync(vehicleViewModel.RegistrationNumber) == null) {
                        var AspNetUserId = await CreateUserAsync(vehicleViewModel.Email, vehicleViewModel.Password, clientIdGuid);
                        var v = await vehicleService.CreateVehicleAsync(new Vehicle {
                            Name = vehicleViewModel.Name,
                            RegistrationNumber = vehicleViewModel.RegistrationNumber,
                            AspNetUserId = AspNetUserId
                        });
                        vehicleResponseViewModel.LoginUserId = vehicleViewModel.Email;
                        message = "Vehicle added successful";
                        status = Status.SUCCESS;
                        code = Code.CREATE;
                    } else {
                        status = Status.WARNING;
                        message = "Registration number already added";
                        code = Code.DATA_ALREADY_EXIST;
                    }
                } catch (Exception ex) {
                    message = ex.Message;
                }
            }

            var response = new ResponseMessage<VehicleResponseViewModel>(status, code, message, vehicleResponseViewModel);

            return Ok(response);
        }

       


        private async Task<string> CreateUserAsync(string email, string password, string clientIdGuid) {

            var user = new UserViewModel {
                ClientId = clientIdGuid.ToGuid().Value,
                UserName = email,
                EmailAddress = email,
                PasswordHash = password,
                Claims = new List<UserClaimViewModel> { new UserClaimViewModel { Type = "appType", Value = "VTUser" } }
            };
            string userId = null;
            using (var httpCLient = new HttpClient()) {
                var jsonData = JsonConvert.SerializeObject(user);
                var userUrl = _idServerUrl + "/api/users";
                var result = httpCLient.PostAsync(userUrl, new StringContent(jsonData, Encoding.UTF8, "application/json")).Result;
                if (result.IsSuccessStatusCode) {
                    dynamic dn = JsonConvert.DeserializeObject(await result.Content.ReadAsStringAsync());
                    userId = dn.id;
                    var userUnLockUrl = _idServerUrl + "/api/users/" + email + "/unlock";
                    result = await httpCLient.PutAsync(userUnLockUrl, new StringContent(string.Empty));
                } else {
                    string ds = await result.Content.ReadAsStringAsync();
                    List<IdentityError> errors = JsonConvert.DeserializeObject<List<IdentityError>>(ds);
                    if (errors != null & errors.Count > 0) {
                        throw new Exception($"User can't create for {user.EmailAddress}, {errors[0].Description}");
                    }
                }
            }

            return userId;
        }

        async Task<string> GetClientAsync(string ClientId) {
            using (var httpCLient = new HttpClient()) {
                var clientUrl = $"{_idServerUrl}/api/clients/{ClientId}";
                var result = await httpCLient.GetAsync(clientUrl);
                var json = await result.Content.ReadAsStringAsync();
                dynamic dn = JsonConvert.DeserializeObject<ExpandoObject>(json);
                if (dn != null) {
                    return dn.data.id;
                } else {
                    return string.Empty;
                }
            }
        }
        
    }
}
