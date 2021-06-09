using IdentityServer4.Services;
using Microsoft.Extensions.Logging;
using System.Linq;
using System.Threading.Tasks;
using VehicleTracking.Interfaces.Repository;

namespace VehicleTracking.IdentityServer.Services {
    public class CorsPolicyService : ICorsPolicyService
    {
        private readonly ILogger<CorsPolicyService> _logger;
        private readonly IRepositoryAsync<Model.ClientCorsOrigin> _clientsRepository;

        public CorsPolicyService(ILogger<CorsPolicyService> logger, IRepositoryAsync<Model.ClientCorsOrigin> clientsRepository )
        {
            _logger = logger;
            _clientsRepository = clientsRepository;
        }

        public async Task<bool> IsOriginAllowedAsync(string origin)
        {
            _logger.LogDebug("CorsPolicyService CORS request made from origin: {origin}", origin);

            var allowedOrigins = await _clientsRepository.FindAllAsync(s => s.Origin == origin);

            var isAllowed = allowedOrigins.Any();
            _logger.LogDebug("CorsPolicyService CORS {origin} IsAllowed {allowed}", origin, isAllowed ? $"Yes": $"No");

            return isAllowed;
        }
    }
}
