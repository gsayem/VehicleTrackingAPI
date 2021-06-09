using IdentityServer4.Models;
using IdentityServer4.Stores;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VehicleTracking.IdentityServer.Mappers;
using VehicleTracking.Interfaces.Repository;

namespace VehicleTracking.IdentityServer.Stores {
    public class ResourceStore : IResourceStore {
        private readonly IRepositoryAsync<Model.ApiResource> _apiResourceRepository;
        private readonly IRepositoryAsync<Model.IdentityResource> _identityResourceRepository;
        private readonly IRepositoryAsync<Model.ApiScope> _identityApiScopeRepository;

        public ResourceStore(IRepositoryAsync<Model.ApiResource> apiResourceRepository,
            IRepositoryAsync<Model.IdentityResource> identityResourceRepository,
            IRepositoryAsync<Model.ApiScope> identityApiScopeRepository) {
            _apiResourceRepository = apiResourceRepository;
            _identityResourceRepository = identityResourceRepository;
            _identityApiScopeRepository = identityApiScopeRepository;
        }

        public async Task<IEnumerable<ApiResource>> FindApiResourcesByNameAsync(IEnumerable<string> apiResourceNames) {
            var id4ApiResources = await _apiResourceRepository.FindAllAsync(s => apiResourceNames.Contains(s.Name));
            return id4ApiResources.Select(x => x.ToModel()).AsEnumerable();
        }

        public async Task<IEnumerable<ApiResource>> FindApiResourcesByScopeNameAsync(IEnumerable<string> scopeNames) {
            var id4ApiResources = await _apiResourceRepository.FindAllAsync(s =>
            s.Scopes.Any(x => scopeNames.Contains(x.Name)));
            return id4ApiResources.Select(x => x.ToModel()).AsEnumerable();
        }

        public async Task<IEnumerable<ApiScope>> FindApiScopesByNameAsync(IEnumerable<string> scopeNames) {
            var id4ApiScope = await _identityApiScopeRepository.FindAllAsync(s => scopeNames.Contains(s.Name));
            return id4ApiScope.Select(x => x.ToModel()).AsEnumerable();
        }

        public async Task<IEnumerable<IdentityResource>> FindIdentityResourcesByScopeNameAsync(IEnumerable<string> scopeNames) {
            var id4Resources = await _identityResourceRepository.FindAllAsync(s => scopeNames.Contains(s.Name));
            return id4Resources.Select(s => s.ToModel()).AsEnumerable();
        }

        public async Task<Resources> GetAllResourcesAsync() {
            var id4Resources = await _identityResourceRepository.GetAllAsync();
            var id4ApiResources = await _apiResourceRepository.GetAllAsync();
            var id4ApiScope = await _identityApiScopeRepository.GetAllAsync();

            var resources = new Resources(
                id4Resources.Select(x => x.ToModel()).AsEnumerable(),
                id4ApiResources.Select(x => x.ToModel()).AsEnumerable(),
                id4ApiScope.Select(x => x.ToModel()).AsEnumerable());
            return resources;
        }
    }
}
