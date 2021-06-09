using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
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
    [ApiController]
    [Route("[controller]")]
    public class VehiclePositionController : ControllerBase {
        private readonly string _idServerUrl = "http://localhost:5000"; //As there is no UI and it's only for swagger and test.
        private readonly ILogger<VehiclePositionController> _logger;

        public VehiclePositionController(ILogger<VehiclePositionController> logger) {
            _logger = logger;
        }

        [HttpPost]
        [ProducesResponseType(typeof(ResponseMessage<VehicleResponseViewModel>), 200)]
        [ProducesResponseType(typeof(ResponseMessage<string>), 400)]
        [ProducesResponseType(typeof(ResponseMessage<string>), 401)]
        [ProducesResponseType(typeof(ResponseMessage<string>), 404)]
        [ProducesResponseType(typeof(ResponseMessage<string>), 403)]
        [AuthorizeAny(ClaimType.AppType.VTUser)]
        public async Task<IActionResult> VehiclePositionRegistration(VehiclePositionViewModel positionViewModel,
                [FromServices] IVehicleService vehicleService,
               [FromServices] IVehiclePositionService vehiclePositionService) {
            string message = string.Empty;
            Status status = Status.FAILURE;
            Code code = Code.CREATE;
            VehicleResponseViewModel vehicleResponseViewModel = new();
            if (positionViewModel == null) {
                message = "Position data not found";
                code = Code.DATA_NOT_FOUND;
            }

            if (message.IsNullOrWhiteSpace() && positionViewModel.Email.IsNullOrWhiteSpace()) {
                message = "Email required";
                code = Code.DATA_NOT_FOUND;
            }
            if (message.IsNullOrWhiteSpace() && positionViewModel.Latitude == null) {
                message = "Latitude required";
                code = Code.DATA_NOT_FOUND;
            }
            if (message.IsNullOrWhiteSpace() && positionViewModel.Longitude == null) {
                message = "Longitude required";
                code = Code.DATA_NOT_FOUND;
            }
            string userId = string.Empty;
            if (message.IsNullOrWhiteSpace()) {
                userId = await GetUserAsync(positionViewModel.Email);
                if (userId.IsNullOrWhiteSpace()) {
                    message = "Invalid email address";
                    code = Code.DATA_NOT_FOUND;
                }
            }

            if (message.IsNullOrWhiteSpace()) {
                try {
                    var vehical = await vehicleService.FindVehicleByAspNetUserIdAsync(userId);
                    if (vehical != null) {

                        var v = await vehiclePositionService.CreateVehiclePositionAsync(new VehiclePosition {
                            Vehicle_Id = vehical.Id,
                            Latitude = positionViewModel.Latitude,
                            Longitude = positionViewModel.Longitude
                        });
                        message = "Vehicle Position added successful";
                        status = Status.SUCCESS;
                        code = Code.CREATE;
                    } else {
                        status = Status.FAILURE;
                        message = "Vehical not found";
                        code = Code.DATA_NOT_FOUND;
                    }
                } catch (Exception ex) {
                    message = ex.Message;
                }
            }

            var response = new ResponseMessage<string>(status, code, message);

            return Ok(response);
        }

        [HttpGet]
        [Route("current/{vehicleid}")]
        [ProducesResponseType(typeof(ResponseMessage<VehiclePosition>), 200)]
        [ProducesResponseType(typeof(ResponseMessage<string>), 400)]
        [ProducesResponseType(typeof(ResponseMessage<string>), 401)]
        [ProducesResponseType(typeof(ResponseMessage<string>), 404)]
        [ProducesResponseType(typeof(ResponseMessage<string>), 403)]
        [AuthorizeAny(ClaimType.AppType.Admin)]
        public async Task<IActionResult> GetVehicleCurrentPosition(string vehicleid, [FromServices] IVehiclePositionService vehiclePositionService) {
            string message = string.Empty;
            Status status = Status.FAILURE;
            Code code = Code.GET;
            VehiclePosition position = new VehiclePosition();
            if (message.IsNullOrWhiteSpace() && vehicleid.IsNullOrWhiteSpace()) {
                message = "vehicle id required";
                code = Code.DATA_NOT_PROVIDED;
            }

            if (message.IsNullOrWhiteSpace()) {
                try {
                    var positionList = await vehiclePositionService.GetVehiclePositionListAsyncByDate(vehicleid.ToGuid().Value, DateTime.Now);
                    position = positionList.OrderByDescending(s => s.CreatedDate).FirstOrDefault();
                    status = Status.SUCCESS;
                } catch (Exception ex) {
                    message = ex.Message;
                }
            }

            var response = new ResponseMessage<VehiclePosition>(status, code, message, position);

            return Ok(response);
        }
        [HttpGet]
        [Route("{vehicleid}/{fromDate}/{toDate}")]
        [ProducesResponseType(typeof(ResponseMessage<List<VehiclePositionViewModel>>), 200)]
        [ProducesResponseType(typeof(ResponseMessage<string>), 400)]
        [ProducesResponseType(typeof(ResponseMessage<string>), 401)]
        [ProducesResponseType(typeof(ResponseMessage<string>), 404)]
        [AuthorizeAny(ClaimType.AppType.Admin)]
        public async Task<IActionResult> GetVehiclePosition(string vehicleid, DateTime fromDate, DateTime toDate, [FromServices] IVehicleService vehicleService,
       [FromServices] IVehiclePositionService vehiclePositionService) {
            string message = string.Empty;
            Status status = Status.FAILURE;
            Code code = Code.CREATE;
            ICollection<VehiclePositionViewModel> positionList = new List<VehiclePositionViewModel>();
            if (message.IsNullOrWhiteSpace() && vehicleid.IsNullOrWhiteSpace()) {
                message = "vehicle id required";
                code = Code.DATA_NOT_PROVIDED;
            }


            if (message.IsNullOrWhiteSpace()) {
                try {
                    var posList = await vehiclePositionService.GetVehiclePositionListAsyncByDateRange(vehicleid.ToGuid().Value, fromDate, toDate);
                    posList.ForEach(s => {
                        positionList.Add(new VehiclePositionViewModel { Longitude = s.Longitude, Latitude = s.Latitude });
                    });

                } catch (Exception ex) {
                    message = ex.Message;
                }
            }

            var response = new ResponseMessage<List<VehiclePositionViewModel>>(status, code, message, positionList.ToList());

            return Ok(response);
        }


        async Task<string> GetUserAsync(string email) {
            using (var httpCLient = new HttpClient()) {
                var clientUrl = $"{_idServerUrl}/api/users/{email}";
                var result = await httpCLient.GetAsync(clientUrl);
                var json = await result.Content.ReadAsStringAsync();
                var dn = JsonConvert.DeserializeObject<UserViewModel>(json);
                if (dn != null) {
                    return dn.UserId.ToString();
                } else {
                    return string.Empty;
                }
            }
        }
    }
}
