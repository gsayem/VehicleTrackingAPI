using System.Collections.Generic;
using System.Threading.Tasks;
using VehicleTracking.IdentityServer.Model;

namespace VehicleTracking.IdentityServer.Services {
    public interface IApplicationUserService {
        Task<IList<PasswordHistory>> GetPasswordHistoriesAsync(ApplicationUser user);
    }
}
