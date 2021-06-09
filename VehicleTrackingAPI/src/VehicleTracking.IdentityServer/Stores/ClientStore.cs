using IdentityServer4.Models;
using IdentityServer4.Stores;
using System.Threading.Tasks;
using VehicleTracking.IdentityServer.Mappers;
using VehicleTracking.Interfaces.Repository;

namespace VehicleTracking.IdentityServer.Stores {
    public class ClientStore : IClientStore {
        private readonly IRepositoryAsync<Model.Client> _clientRepository;

        public ClientStore(IRepositoryAsync<Model.Client> clientRepository) {
            _clientRepository = clientRepository;
        }

        public async Task<Client> FindClientByIdAsync(string clientId) {
            var client = await _clientRepository.FindAsync(c => c.ClientId == clientId);
            return client?.ToModel();
        }
    }
}
