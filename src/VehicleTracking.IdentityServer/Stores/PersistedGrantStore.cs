using IdentityServer4.Models;
using IdentityServer4.Stores;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VehicleTracking.Interfaces.Repository;
using VehicleTracking.IdentityServer.Mappers;

namespace VehicleTracking.IdentityServer.Stores {
    public class PersistedGrantStore : IPersistedGrantStore {
        private readonly IRepositoryAsync<Model.PersistedGrant> _persistedGrantRepository;

        public PersistedGrantStore(IRepositoryAsync<Model.PersistedGrant> persistedGrantRepository) {
            _persistedGrantRepository = persistedGrantRepository;
        }

        public async Task StoreAsync(PersistedGrant grant) {
            var grants = await _persistedGrantRepository.FindAllAsync(s => s.Key == grant.Key);

            if (grants.Any()) {
                var existing = grants.SingleOrDefault();
                await _persistedGrantRepository.UpdateAsync(existing);
            } else {
                var persistedGrant = grant.ToEntity();
                await _persistedGrantRepository.AddAsync(persistedGrant);
            }
        }

        public async Task<PersistedGrant> GetAsync(string key) {
            var grants = await _persistedGrantRepository.FindAllAsync(s => s.Key == key);
            var grant = grants.SingleOrDefault();

            return grant?.ToModel();
        }

        public async Task<IEnumerable<IdentityServer4.Models.PersistedGrant>> GetAllAsync(PersistedGrantFilter filter) {
            var persistedGrants = await _persistedGrantRepository.FindAllAsync(s => s.SubjectId == filter.SubjectId);

            var model = persistedGrants.Select(x => x.ToModel());
            return model;
        }

        public async Task RemoveAsync(string key) {
            var grants = await _persistedGrantRepository.FindAllAsync(s => s.Key == key);
            if (grants.Any()) {
                await _persistedGrantRepository.RemoveAsync(grants.Single());
            }
        }

        public async Task RemoveAllAsync(PersistedGrantFilter filter) {
            var persistedGrants = await _persistedGrantRepository.FindAllAsync(s => s.SubjectId == filter.SubjectId && s.ClientId == filter.ClientId);

            foreach (var grant in persistedGrants) {
                await _persistedGrantRepository.RemoveAsync(grant);
            }
        }

        public async Task RemoveAllAsync(string subjectId, string clientId, string type) {
            var persistedGrants = await _persistedGrantRepository.FindAllAsync(s => s.SubjectId == subjectId && s.ClientId == clientId && s.Type == type);

            foreach (var grant in persistedGrants) {
                await _persistedGrantRepository.RemoveAsync(grant);
            }
        }
    }
}
