using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using VehicleTracking.IdentityServer.Model;
using VehicleTracking.Web.Common.ViewModels;

namespace VehicleTracking.Seeder {
    public class SeedDefaultData : SeedBase {
        public SeedDefaultData() {

        }
        public async Task CreateDeafultClientAsync() {

            var file = File.OpenRead(Path.Combine(Utils.GetProjectPath("VehicleTracking.Seeder"), "defaultData.json"));
            var reader = new StreamReader(file);
            var jsonData = reader.ReadToEnd();
            dynamic json = JArray.Parse(jsonData)[0];
            
            //Client Secret Code and Bearer Token Generate

            await CreateClientAsync(json.defaultClient.ClientId.Value, json.defaultClient.Name.Value); //Create client
            //string clientsecretCode = await GenerateClientSecretAsync(json.defaultClient.ClientId.Value);
            SaveClientSecret(json.defaultClient.ClientId.Value, json.defaultClient.ClientsSecretAfterSHA256.Value);


            Client tempClient = await CreateClientRelatedData(json.defaultClient.ClientId.Value, json.defaultClient);
            //User
            var claims = PrepareUserClaims(json.defaultClient);
            await CreateUserAsync(json.defaultClient.User.Email.ToString(), json.defaultClient.User.Password.ToString(), tempClient.Id, claims);

            await SetBearerTokenAsync(json.defaultClient.ClientId.Value, json.defaultClient.CustomerSecret.Value, json.defaultClient.User.Email.ToString(), json.defaultClient.User.Password.ToString());

            

        }
    }
}
