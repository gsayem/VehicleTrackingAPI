using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using VehicleTracking.Common.Extension;
using VehicleTracking.IdentityServer.Model;
using VehicleTracking.IdentityServer.Services;
using VehicleTracking.IdentityServer.Web.API.Helpers;
using VehicleTracking.IdentityServer.Web.API.Services;
using VehicleTracking.Web.Common.ViewModels;

namespace VehicleTracking.IdentityServer.Web.API {

    [ApiController]
    [Produces("application/json")]
    public class UserController : Controller {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IClientService _clientService;
        private readonly IOptions<UserPasswordOptions> _userPasswordOptions;
        private readonly IUtilHelper _utilHelper;
        private readonly IEmailSender _emailSender;
        public UserController(UserManager<ApplicationUser> userManager, IEmailSender emailSender, IUtilHelper utilHelper,
            IOptions<UserPasswordOptions> userPasswordOptions,
            IClientService clientService) {
            _userManager = userManager;
            _userPasswordOptions = userPasswordOptions;
            _clientService = clientService;
            _emailSender = emailSender;
            _utilHelper = utilHelper;
        }

        [HttpPost]
        [ProducesResponseType(typeof(ResponseMessage<string>), 201)]
        [ProducesResponseType(typeof(ResponseMessage<string>), 400)]
        [ProducesResponseType(typeof(ResponseMessage<string>), 500)]
        [Route("api/users")]
        [AllowAnonymous]
        public async Task<IActionResult> Post([FromBody] UserViewModel user) {

            if (user == null) {
                return BadRequest(new ResponseMessage<string>(Status.FAILURE, Code.DATA_NOT_PROVIDED, "Please provide user detail."));
            }
            if (string.IsNullOrWhiteSpace(user.EmailAddress)) {
                return BadRequest(new ResponseMessage<string>(Status.FAILURE, Code.DATA_NOT_PROVIDED, "Email address required."));
            }
            var currentUser = new ApplicationUser() {
                UserName = user.EmailAddress,
                Email = user.EmailAddress,
                NormalizedEmail = user.EmailAddress.ToUpper(),
                EmailConfirmed = true, // let's make it true for now.
                TwoFactorEnabled = false,
                LockoutEnabled = false,
                ClientId = user.ClientId,
                ForceResetPassword = user.ForceResetPassword,
                PasswordExpireTime = DateTime.UtcNow.AddDays(_userPasswordOptions.Value.PasswordExpirationInDay)
            };
            IdentityResult identityResult = null;
            try {
                identityResult = await _userManager.CreateAsync(currentUser, string.IsNullOrWhiteSpace(user.PasswordHash) ? _userPasswordOptions.Value.DefaultPassword : user.PasswordHash);
                if (identityResult.Succeeded) {
                    if (user.Claims != null) {
                        List<Claim> objClaimList = new List<Claim>();
                        user.Claims.ForEach(c => {
                            objClaimList.Add(new Claim(c.Type, c.Value));
                        });
                        if (objClaimList.Count > 0) {
                            await _userManager.AddClaimsAsync(currentUser, objClaimList);
                        }

                        if (currentUser.PasswordHistories == null) {
                            currentUser.PasswordHistories = new List<PasswordHistory>();
                        }

                        currentUser.PasswordHistories.Add(new PasswordHistory { PasswordHash = currentUser.PasswordHash });
                        await _userManager.UpdateAsync(currentUser);
                    }
                    //var code = await _userManager.GenerateEmailConfirmationTokenAsync(currentUser);
                    //var callbackUrl = Url.Action("ConfirmEmail", "Account", new { userId = currentUser.Id, code }, protocol: HttpContext.Request.Scheme);
                    //string emailSubject = "Confirm your account";
                    //dynamic expandoObject = new ExpandoObject();
                    //expandoObject.Subject = emailSubject;
                    //expandoObject.CallbackUrl = callbackUrl;
                    //expandoObject.BaseUrl = _utilHelper.BaseUrl(Request);
                    //string htmlMessage = await _utilHelper.ParseTemplateAsync("ConfirmEmail.cshtml", null, expandoObject);
                    //await _emailSender.SendEmailAsync(currentUser.Email, emailSubject, htmlMessage);
                } else {
                    return StatusCode(500, identityResult.Errors);
                }
            } catch (Exception) {
                return StatusCode(500, identityResult.Errors);
            }

            return Created(_utilHelper.BaseUrl(Request) + "/api/users/" + currentUser.Id, new { currentUser.Id });
        }

        [HttpPut]
        [ProducesResponseType(404)]
        [ProducesResponseType(200)]
        [Route("api/users/{email}")]
        public async Task<IActionResult> Put([FromRoute] string email, [FromBody] UserViewModel user) {
            var currentUser = await _userManager.FindByEmailAsync(email);
            if (currentUser != null) {
                var existingClaims = await _userManager.GetClaimsAsync(currentUser);
                if (user.Claims != null) {
                    List<Claim> addClaims = new();
                    List<Claim> delClaims = new();
                    user.Claims.ForEach(c => {
                        if (existingClaims?.SingleOrDefault(s => s.Type == c.Type && s.Value == c.Value) == null) {
                            addClaims.Add(new Claim(c.Type, c.Value));
                        }
                    });
                    existingClaims?.ForEach(e => {
                        if (user.Claims.SingleOrDefault(s => s.Type == e.Type && s.Value == e.Value) == null) {
                            delClaims.Add(new Claim(e.Type, e.Value));
                        }
                    });
                    if (delClaims.Count > 0) {
                        await _userManager.RemoveClaimsAsync(currentUser, delClaims);
                    }
                    if (addClaims.Count > 0) {
                        await _userManager.AddClaimsAsync(currentUser, addClaims);
                    }
                } else {
                    if (existingClaims != null) {
                        await _userManager.RemoveClaimsAsync(currentUser, existingClaims);
                    }
                }
                return Ok(new ResponseMessage<string>(Status.SUCCESS, Code.UPDATE, "Update successfully"));
            }
            return NotFound(new ResponseMessage<string>(Status.FAILURE, Code.DATA_NOT_FOUND, "Notfound"));
        }

        [HttpGet]
        [Route("api/users/{email}")]
        public async Task<IActionResult> Get([FromRoute] string email) {
            ApplicationUser currentUser = await _userManager.FindByEmailAsync(email);
            if (currentUser != null) {
                UserViewModel user = new UserViewModel();
                user.ClientId = currentUser.ClientId;
                user.UserId = currentUser.Id;
                user.EmailAddress = currentUser.Email;
                user.IsActive = !currentUser.LockoutEnabled;
                user.UserName = currentUser.UserName;
                user.ForceResetPassword = currentUser.ForceResetPassword;

                var existingClaims = await _userManager.GetClaimsAsync(currentUser);
                if (existingClaims != null) {
                    user.IsAdmin = existingClaims.Any(c => c.Type == "Entity_Role" && c.Value == "Administrator");
                    user.Claims = new List<UserClaimViewModel>();
                    existingClaims.ForEach(c => {
                        user.Claims.Add(new UserClaimViewModel { Type = c.Type, Value = c.Value });
                    });
                }
                return Ok(user);
            } else {
                return NotFound();
            }
        }


        [HttpPut]
        [AllowAnonymous]
        [Route("api/users/{email}/lock")]
        public async Task<IActionResult> Lock(string email) {
            ApplicationUser currentUser = await _userManager.FindByEmailAsync(email);
            if (currentUser != null) {
                await _userManager.SetLockoutEnabledAsync(currentUser, true);
                return Ok(new ResponseMessage<string>(Status.SUCCESS, Code.UPDATE, "Lock successfully"));
            } else {
                return NotFound();
            }
        }
        [HttpPut]
        [AllowAnonymous]
        [Route("api/users/{email}/unlock")]
        public async Task<IActionResult> UnLock(string email) {
            ApplicationUser currentUser = await _userManager.FindByEmailAsync(email);
            if (currentUser != null) {
                await _userManager.SetLockoutEnabledAsync(currentUser, false);
                return Ok(new ResponseMessage<string>(Status.SUCCESS, Code.UPDATE, "Unlock successfully"));
            } else {
                return NotFound();
            }
        }
        [HttpPut]
        [AllowAnonymous]
        [Route("api/users/{email}/forcepasswordreset")]
        public async Task<IActionResult> ForcePasswordReset(string email) {
            ApplicationUser currentUser = await _userManager.FindByEmailAsync(email);
            if (currentUser != null) {
                currentUser.ForceResetPassword = true;
                var identityResult = await _userManager.UpdateAsync(currentUser);
                if (identityResult.Succeeded) {
                    return Ok(new ResponseMessage<string>(Status.SUCCESS, Code.UPDATE, "Successfully"));

                } else {
                    return BadRequest(identityResult.Errors);
                }
            } else {
                return NotFound();
            }
        }

        [HttpPost]
        [AllowAnonymous]
        [Route("api/users/{email}/forgotpassword")]
        public async Task<IActionResult> ForgotPassword(string email) {
            var user = await _userManager.FindByNameAsync(email);
            if (user == null || !(await _userManager.IsEmailConfirmedAsync(user))) {
                // Don't reveal that the user does not exist or is not confirmed
                return Ok(new ResponseMessage<string>(Status.SUCCESS, Code.UPDATE, "Successfully"));
            }

            var code = await _userManager.GeneratePasswordResetTokenAsync(user);
            var callbackUrl = Url.Action("ResetPassword", "Account", new { userId = user.Id, code = code }, protocol: HttpContext.Request.Scheme);
            string emailSubject = "Reset Password";

            dynamic expandoObject = new ExpandoObject();
            expandoObject.Subject = emailSubject;
            expandoObject.CallbackUrl = callbackUrl;
            expandoObject.BaseUrl = _utilHelper.BaseUrl(Request);

            string htmlMessage = await _utilHelper.ParseTemplateAsync("ResetPasswordEmail.cshtml", null, expandoObject);
            await _emailSender.SendEmailAsync(user.Email, emailSubject, htmlMessage);

            return Ok(new ResponseMessage<string>(Status.SUCCESS, Code.UPDATE, "Successfully"));
        }
    }
}