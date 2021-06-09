using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using VehicleTracking.Common.Extension;
using VehicleTracking.Interfaces.Repository;
using VehicleTracking.Interfaces.Services;
using VehicleTracking.Model;

namespace VehicleTracking.Services {
    public class VehiclePositionService : IVehiclePositionService {
        private readonly IUnitOfWork _unitOfWork;
        IRepositoryAsync<Vehicle> _vehicleRepository;
        IRepositoryAsync<VehiclePosition> _vehiclePositionRepository;
        public VehiclePositionService(IUnitOfWork unitOfWork, IRepositoryAsync<Vehicle> vehicleRepository, IRepositoryAsync<VehiclePosition> vehiclePositionRepository) {
            _unitOfWork = unitOfWork;
            _vehicleRepository = vehicleRepository;
            _vehiclePositionRepository = vehiclePositionRepository;
        }
        public async Task<VehiclePosition> CreateVehiclePositionAsync(VehiclePosition vehiclePosition) {
            if (vehiclePosition.Vehicle_Id.ToString().IsNullOrWhiteSpace()) {
                throw new Exception("Vehicle required");
            }

            await _vehiclePositionRepository.AddAsync(vehiclePosition);
            return vehiclePosition;
        }

        public Task<VehiclePosition> DeleteVehiclePositionAsync(VehiclePosition vehiclePosition) {
            throw new Exception("Vehicle position delete not acceptable");
        }

        public async Task<ICollection<VehiclePosition>> GetVehiclePositionListAsyncByDate(Guid vehicleId, DateTime date) {
            return await _vehiclePositionRepository.FindAllAsync(s => s.Vehicle_Id == vehicleId && s.CreatedDate == date);
        }

        public async Task<ICollection<VehiclePosition>> GetVehiclePositionListAsyncByDateRange(Guid vehicleId, DateTime fromDatem, DateTime toDate) {
            return await _vehiclePositionRepository.FindAllAsync(s => s.Vehicle_Id == vehicleId && s.CreatedDate >= fromDatem && s.CreatedDate <= toDate);
        }

        public async Task<ICollection<VehiclePosition>> GetVehiclePositionListAsyncByVehicleId(Guid vehicleId) {
            return await _vehiclePositionRepository.FindAllAsync(s => s.Vehicle_Id == vehicleId);
        }

        public Task<VehiclePosition> UpdateVehiclePositionAsync(VehiclePosition vehiclePosition) {
            throw new Exception("Vehicle position update not acceptable");
        }
    }
}
