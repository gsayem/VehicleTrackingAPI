using System;
using System.Threading.Tasks;
using VehicleTracking.IdentityServer.Model;

namespace VehicleTracking.IdentityServer.Services {
    public interface IClientService {
        Task<Client> FindByClientIdAsync(string clientId);
        Task<ClientSecret> FindClientSecretByClient(Client client);
        Task<Guid> SaveClientSecret(ClientSecret clientSecret);
        Task<Client> FindByClientId(string clientId);
        Task<Guid> AddClient(Client client);
        Task<Guid> UpdateClient(Client client);
        Task<bool> IsUniqueClient(string clientId);
    }
}
