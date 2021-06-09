using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.IO;
using System.Linq;
using System.Text;

namespace VehicleTracking.Seeder {
    class Program {
        public const string LocalHost = "localhost";
        public const string Staging = "Staging";
        public const string Production = "Production";
        public const string UAT = "UAT";

        static async System.Threading.Tasks.Task Main(string[] args) {

            Console.Title = "Vehicle Tracking Seed Client";
            // Initialise
            var connection = ChooseEnvironment();
            if (connection != LocalHost) {
                Log.Error("Only localhost allow!");
                Console.ReadLine();
                return;
            }

            SetEnvironmentVariables(connection);
            var seedIDServer = new SeedIdentityServer();
            var defaultSeed = new SeedDefaultData();
            var seedData = new SeedData();

            // Database Migrations
            var mainDataMigrator = new DatabaseMigrator(Utils.GetProjectPath("VehicleTracking.DataMigration.App"));
            var identityDataMigrator = new DatabaseMigrator(Utils.GetProjectPath("VehicleTracking.DataMigration.Identity.App"));

            dynamic data = GetSeedData();

            Log.Info("Dropping main database");
            mainDataMigrator.DropDatabase();

            Log.Info("Dropping Identity Server database");
            identityDataMigrator.DropDatabase();

            Log.Info("Migrating main database");
            mainDataMigrator.MigrateDatabase();

            Log.Info("Migrating Identity Server database");
            identityDataMigrator.MigrateDatabase();

            // Basic seeding
            Log.Info("Running IdServerSetup");
            seedIDServer.IdServerSetup();

            await defaultSeed.CreateDeafultClientAsync();

            await seedData.CreateClientsAndUsers(data);

        }
        private static string ChooseEnvironment() {
            var selectedName = LocalHost;

            Log.Info("Please select the environment to seed and ensure you are connected to the relevant environment:");
            Log.Info($"===============================================================================================");
            Log.Info($"1: {LocalHost}");
            Log.Info($"2: {Staging}");
            Log.Info($"3: {UAT}");
            Log.Info($"4: {Production}");

            string consoleLine = Console.ReadLine();

            switch (consoleLine) {
                case "2":
                    selectedName = Staging;
                    break;
                case "3":
                    selectedName = UAT;
                    break;
                case "4":
                    selectedName = Production;
                    break;
                default:
                    selectedName = LocalHost;
                    break;
            }

            Log.Info("\nSeeding " + selectedName);

            return selectedName;
        }

        private static void SetEnvironmentVariables(string connection) {
            //string sqlExtension = "";
            string fileName = "envVars.json";


            var file = File.OpenRead(fileName);
            var reader = new StreamReader(file);
            var jsonData = reader.ReadToEnd();

            var environments = JsonConvert.DeserializeObject<Environments>(jsonData);

            var variables = environments.Current.FirstOrDefault(e => e.Name == connection);

            Environment.SetEnvironmentVariable(EnvironmentVariableKeys.ApiSqlConnection, variables.ApiSqlConnection);
            Environment.SetEnvironmentVariable(EnvironmentVariableKeys.ApiUrl, variables.ApiUrl);
            Environment.SetEnvironmentVariable(EnvironmentVariableKeys.IdentitySqlConnection, variables.IdentitySqlConnection);
            Environment.SetEnvironmentVariable(EnvironmentVariableKeys.IdentityUrl, variables.IdentityUrl);
        }

        private static dynamic GetSeedData() {
            string filesPath = Path.Combine(Utils.GetProjectPath("VehicleTracking.Seeder"), "SeedData");
            StringBuilder jsonDataSB = null;
            Log.Info("\nLoading seed data");

            jsonDataSB = ReadAllJsonFile(filesPath);
            string jsonData = "{\"clients\":[]}";
            if (jsonDataSB != null) {
                jsonData = "{\"clients\":[" + jsonDataSB.ToString() + "]}";
            }
            return JObject.Parse(jsonData);
        }
        private static StringBuilder ReadAllJsonFile(string pPath) {
            FileStream file = null;
            StreamReader reader = null;
            StringBuilder jsonDataSB = null;
            string jsonDataTemp;
            JObject jObject = null;
            Directory.EnumerateFiles(pPath, "*.json", SearchOption.TopDirectoryOnly).ToList().ForEach(f => {
                try {
                    jsonDataTemp = string.Empty;
                    file = File.OpenRead(f);
                    reader = new StreamReader(file);
                    jsonDataTemp = reader.ReadToEnd();

                    jObject = JObject.Parse(jsonDataTemp); // Check for valid json.

                    if (jObject != null && jObject.Count > 0) {
                        if (jsonDataSB == null) {
                            jsonDataSB = new StringBuilder();
                            jsonDataSB.Append(jsonDataTemp);
                        }
                        else {
                            jsonDataSB.Append(",").Append(jsonDataTemp);
                        }
                    }
                }
                catch {
                    Log.Error($"\nError reading to file: {Path.GetFileName(f)}");
                }
            });
            return jsonDataSB;
        }
    }
}
