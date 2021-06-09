using System.Collections.Generic;
using System.Threading.Tasks;
using VehicleTracking.IdentityServer.Model;
using VehicleTracking.Interfaces.Repository;

namespace VehicleTracking.IdentityServer.Services {
    public class ApplicationUserService : IApplicationUserService
    {
        private readonly IRepositoryAsync<PasswordHistory> _userPasswordHistoryRepository;
        public ApplicationUserService(IRepositoryAsync<PasswordHistory> userPasswordHistoryRepository)
        {
            _userPasswordHistoryRepository = userPasswordHistoryRepository;
        }

        public async Task<IList<PasswordHistory>> GetPasswordHistoriesAsync(ApplicationUser user)
        {
            return await _userPasswordHistoryRepository.FindAllAsync(s => s.UserId == user.Id);
        }
    }
}
