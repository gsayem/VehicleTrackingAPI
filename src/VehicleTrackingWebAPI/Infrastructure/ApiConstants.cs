using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace VehicleTracking.WebAPI.Infrastructure {
    public class ApiConstants {
        public class AuthorizationPolicy {
            public const string MustBeAdmin = "MustBeAdmin";
            public const string MustBeVTUser = "MustBeVTUser";
        }
        public class ClaimType {
            public const string AppTypeKey = "appType";
            /// <summary>
            /// Constants for user/application type (appType) claims.
            /// </summary>
            public class AppType {
                public const string Admin = "Admin";
                public const string VTUser = "VTUser";
            }
        }
    }
}
