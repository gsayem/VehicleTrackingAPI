using Microsoft.AspNetCore.Mvc;
using System;

namespace VehicleTracking.WebAPI.AuthorizationHandlers {
    public class AuthorizeAnyAttribute : TypeFilterAttribute {
        public AuthorizeAnyAttribute(params string[] policies) : base(typeof(PolicyRequirementFilter)) {
            if (policies == null || policies.Length == 0) {
                throw new ArgumentNullException(nameof(policies), "At least one claim must be supplied");
            }

            Arguments = new object[] { policies };
        }
    }
}
