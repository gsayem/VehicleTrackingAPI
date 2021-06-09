using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VehicleTracking.Seeder {
    internal sealed class EnvironmentVariableKeys {
        public const string ApiSqlConnection = "VT_API_SQL_CONNECTION";
        public const string ApiUrl = "VT_API_URL";
        public const string IdentitySqlConnection = "VT_IDENTITY_SQL_CONNECTION";
        public const string IdentityUrl = "VT_IDENTITY_URL";
    }
    public class EnvironmentVariables {
        public string Name { get; set; }
        public string ApiSqlConnection { get; set; }
        public string ApiUrl { get; set; }
        public string IdentitySqlConnection { get; set; }
        public string IdentityUrl { get; set; }

    }
    public class Environments {
        public List<EnvironmentVariables> Current { get; set; }
    }
}
