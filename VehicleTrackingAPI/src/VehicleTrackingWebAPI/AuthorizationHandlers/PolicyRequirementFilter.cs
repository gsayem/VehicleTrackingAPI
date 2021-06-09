using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Linq;
using System.Threading.Tasks;
using static VehicleTracking.WebAPI.Infrastructure.ApiConstants;

namespace VehicleTracking.WebAPI.AuthorizationHandlers {
    public class PolicyRequirementFilter : IAsyncActionFilter {
        private readonly string[] _policies;

        public PolicyRequirementFilter(string[] policies) {
            _policies = policies;
        }

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next) {
            if (context.HttpContext.User.Claims.Any(c => c.Type == ClaimType.AppTypeKey && _policies.Contains(c.Value))) {
                await next();
            }
            else {
                context.Result = new ForbidResult();
            }
        }
    }
}
