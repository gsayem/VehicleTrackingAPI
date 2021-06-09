using System;
using System.Linq;
using System.Threading.Tasks;
using VehicleTracking.Common.Extension;
using VehicleTracking.IdentityServer.Model;
using VehicleTracking.Interfaces.Repository;

namespace VehicleTracking.IdentityServer.Services {
    public class ClientService : IClientService {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IRepositoryAsync<Client> _clientRepository;
        private readonly IRepositoryAsync<ClientSecret> _clientSecretRepository;
        private readonly IRepositoryAsync<ClientClaim> _clientClaimRepository;

        public ClientService(IUnitOfWork unitOfWork,
            IRepositoryAsync<Client> clientRepository,
            IRepositoryAsync<ClientSecret> clientSecretRepository,
            IRepositoryAsync<ClientClaim> clientClaimRepository) {
            _unitOfWork = unitOfWork;
            _clientRepository = clientRepository;
            _clientSecretRepository = clientSecretRepository;
            _clientClaimRepository = clientClaimRepository;
        }
        public async Task<Guid> SaveClientSecret(ClientSecret clientSecret) {
            if (clientSecret.Id.IsNotNull()) {
                await _clientSecretRepository.UpdateAsync(clientSecret);
            } else {
                await _clientSecretRepository.AddAsync(clientSecret);
            }

            return clientSecret.Id;
        }
        public async Task<Client> FindByClientIdAsync(string clientId) {
            return await _clientRepository.FindAsync(cl => cl.ClientId == clientId);
        }
        public async Task<ClientSecret> FindClientSecretByClient(Client client) {
            return await _clientSecretRepository.FindAsync(cl => cl.Client.Id == client.Id);
        }
        public async Task<Client> FindByClientId(string clientId) {
            return await _clientRepository.FindAsync(s => s.ClientId == clientId);
        }
        public async Task<Guid> AddClient(Client client) {
            await _clientRepository.AddAsync(client);
            return client.Id;
        }
        public async Task<Guid> UpdateClient(Client client) {
            var uowClientRepository = _unitOfWork.GetRepositoryAsync<Client>();
            var uowClientGrantTypeRepository = _unitOfWork.GetRepositoryAsync<ClientGrantType>();
            var uowClientScopeRepository = _unitOfWork.GetRepositoryAsync<ClientScope>();

            var existingClient = await uowClientRepository.GetByIdAsync(client.Id);
            existingClient.ClientId = client.ClientId;
            existingClient.ClientName = client.ClientId;

            ClientGrantType tempClientGrantType = null;
            if (client.AllowedGrantTypes != null && client.AllowedGrantTypes.Count > 0) {
                existingClient.AllowedGrantTypes.Reverse().ForEach(async eg => {
                    tempClientGrantType = client.AllowedGrantTypes.Where(cg => cg.GrantType == eg.GrantType).SingleOrDefault();
                    if (tempClientGrantType == null) {
                        existingClient.AllowedGrantTypes.Remove(eg);
                        await uowClientGrantTypeRepository.RemoveAsync(eg);
                    }
                });
                client.AllowedGrantTypes.ForEach(async cg => {
                    tempClientGrantType = existingClient.AllowedGrantTypes.Where(eg => eg.GrantType == cg.GrantType).SingleOrDefault();
                    if (tempClientGrantType == null) {
                        existingClient.AllowedGrantTypes.Add(cg);
                        await uowClientGrantTypeRepository.AddAsync(cg);
                    }
                });
            } else {
                existingClient.AllowedGrantTypes.Reverse().ForEach(async eg => {
                    existingClient.AllowedGrantTypes.Remove(eg);
                    await uowClientGrantTypeRepository.RemoveAsync(eg);
                });
            }

            ClientScope tempClientScope = null;
            if (client.AllowedScopes != null && client.AllowedScopes.Count > 0) {
                existingClient.AllowedScopes.Reverse().ForEach(async eg => {
                    tempClientScope = client.AllowedScopes.Where(cg => cg.Scope == eg.Scope).SingleOrDefault();
                    if (tempClientScope == null) {
                        existingClient.AllowedScopes.Remove(eg);
                        await uowClientScopeRepository.RemoveAsync(eg);
                    }
                });
                client.AllowedScopes.ForEach(async cg => {
                    tempClientScope = existingClient.AllowedScopes.Where(eg => eg.Scope == cg.Scope).SingleOrDefault();
                    if (tempClientScope == null) {
                        existingClient.AllowedScopes.Add(cg);
                        await uowClientScopeRepository.AddAsync(cg);
                    }
                });
            } else {
                existingClient.AllowedScopes.Reverse().ForEach(async eg => {
                    existingClient.AllowedScopes.Remove(eg);
                    await uowClientScopeRepository.RemoveAsync(eg);
                });
            }

            await _unitOfWork.SaveChangesAsync();
            return client.Id;
        }
        public async Task<bool> IsUniqueClient(string clientId) {
            var duplicateClient = await _clientRepository.FindAllAsync(c => c.ClientId == clientId);
            return duplicateClient.Count == 0;
        }

    }
}
