using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using VehicleTracking.Model;

namespace VehicleTracking.Interfaces.Services {
    public interface IVehicleService {
        Task<Vehicle> CreateVehicleAsync(Vehicle vehicle);
        Task<Vehicle> UpdateVehicleAsync(Vehicle vehicle);
        Task<Vehicle> DeleteVehicleAsync(Vehicle vehicle);
        Task<Vehicle> FindVehicleAsync(Guid vehicleId);
        Task<Vehicle> FindVehicleByAspNetUserIdAsync(string userId);
        Task<Vehicle> FindVehicleByRegistrationNumberAsync(string registrationNumber);
        Task<ICollection<Vehicle>> GetVehicleListAsync();
    }
}
