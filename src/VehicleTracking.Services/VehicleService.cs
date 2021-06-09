using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using VehicleTracking.Common.Extension;
using VehicleTracking.Interfaces.Repository;
using VehicleTracking.Interfaces.Services;
using VehicleTracking.Model;

namespace VehicleTracking.Services {
    public class VehicleService : IVehicleService {
        private readonly IUnitOfWork _unitOfWork;
        IRepositoryAsync<Vehicle> _vehicleRepository;

        public VehicleService(IUnitOfWork unitOfWork, IRepositoryAsync<Vehicle> vehicleRepository) {
            _unitOfWork = unitOfWork;
            _vehicleRepository = vehicleRepository;

        }

        public async Task<Vehicle> CreateVehicleAsync(Vehicle vehicle) {
            if (vehicle.Name.IsNullOrWhiteSpace()) {
                throw new Exception("Name required");
            }
            if (vehicle.RegistrationNumber.IsNullOrWhiteSpace()) {
                throw new Exception("Registration Number required");
            }
            await _vehicleRepository.AddAsync(vehicle);
            return vehicle;
        }

        public async Task<Vehicle> DeleteVehicleAsync(Vehicle vehicle) {
            var uowVehicleRepository = _unitOfWork.GetRepositoryAsync<Vehicle>();
            var uowVehiclePositionRepository = _unitOfWork.GetRepositoryAsync<VehiclePosition>();
            foreach (var item in vehicle.VehiclePositions) {
                await uowVehiclePositionRepository.RemoveAsync(item);
            }
            await uowVehicleRepository.RemoveAsync(vehicle);
            await _unitOfWork.SaveChangesAsync();
            return vehicle;
        }

        public async Task<Vehicle> FindVehicleAsync(Guid vehicleId) {
            return await _vehicleRepository.GetByIdAsync(vehicleId);
        }

        public async Task<Vehicle> FindVehicleByRegistrationNumberAsync(string registrationNumber) {
            return await _vehicleRepository.FindAsync(v => v.RegistrationNumber == registrationNumber);
        }

        public async Task<ICollection<Vehicle>> GetVehicleListAsync() {
            return await _vehicleRepository.GetAllAsync();
        }

        public async Task<Vehicle> UpdateVehicleAsync(Vehicle vehicle) {
            if (vehicle.Name.IsNullOrWhiteSpace()) {
                throw new Exception("Name required");
            }
            if (vehicle.RegistrationNumber.IsNullOrWhiteSpace()) {
                throw new Exception("Registration Number required");
            }
            await _vehicleRepository.UpdateAsync(vehicle);
            return vehicle;
        }

        public async Task<Vehicle> FindVehicleByAspNetUserIdAsync(string userId) {
            return await _vehicleRepository.FindAsync(v => v.AspNetUserId == userId);
        }
    }
}
