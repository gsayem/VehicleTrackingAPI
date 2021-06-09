using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace VehicleTracking.Seeder {
    public class Utils {
        public enum ServerStatus {
            NotStarted = 0,
            Starting = 1,
            Running = 2,
            Finished = 3
        }
        
        public static string GetProjectPath(string projectFolderName) {
            if (projectFolderName == null) {
                throw new ArgumentNullException(nameof(projectFolderName));
            }

            return Path.Combine(GetRootPath(), "src", projectFolderName);
        }
        public static string GetRootPath() {
            return AppContext.BaseDirectory.Substring(0, AppContext.BaseDirectory.LastIndexOf(Path.DirectorySeparatorChar + "src" + Path.DirectorySeparatorChar, StringComparison.Ordinal));
        }
        public static FormUrlEncodedContent GetFormEncodedContent(string clientId, string clientSecretCode, string username, string password) {
            var keyValues = new[]{
                new KeyValuePair<string, string>("client_id",  clientId),
                new KeyValuePair<string, string>("client_secret", clientSecretCode),
                new KeyValuePair<string, string>("username", username),
                new KeyValuePair<string, string>("password", password),
                new KeyValuePair<string, string>("grant_type", "password"),
                new KeyValuePair<string, string>("response_type", "token"),
                new KeyValuePair<string, string>("scope", "VehicleTrackingAPI")
           };

            return new FormUrlEncodedContent(keyValues);
        }
    }
}
