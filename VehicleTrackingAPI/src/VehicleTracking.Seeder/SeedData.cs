using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using VehicleTracking.IdentityServer.Model;
using VehicleTracking.Web.Common.ViewModels;

namespace VehicleTracking.Seeder {
    public class SeedData : SeedBase {

        public SeedData() {

        }
        public async Task CreateClientsAndUsers(dynamic data) {

            int index = 0;

            if (data != null & data.clients != null) {
                foreach (var client in data.clients) {
                    await CreateClientAsync(client.ClientId.Value, client.ClientId.Value); //Create client
                    SaveClientSecret(client.ClientId.Value, client.ClientsSecretAfterSHA256.Value);
                    Client tempClient = await CreateClientRelatedData(client.ClientId.Value, client);

                    var claims = PrepareUserClaims(client);
                    await CreateUserAsync(client.User.Email.ToString(), client.User.Password.ToString(), tempClient.Id, claims);
                    Log.InfoInSameLine($"Client created {++index} out of {data.clients.Count}");
                }

            }
        }

    }
}

