using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using VehicleTracking.Model;

namespace VehicleTracking.Interfaces.Services {
    public interface IVehiclePositionService {
        Task<VehiclePosition> CreateVehiclePositionAsync(VehiclePosition vehiclePosition);
        Task<VehiclePosition> UpdateVehiclePositionAsync(VehiclePosition vehiclePosition);
        Task<VehiclePosition> DeleteVehiclePositionAsync(VehiclePosition vehiclePosition);
        Task<ICollection<VehiclePosition>> GetVehiclePositionListAsyncByVehicleId(Guid vehicleId);
        Task<ICollection<VehiclePosition>> GetVehiclePositionListAsyncByDate(Guid vehicleId, DateTime date);
        Task<ICollection<VehiclePosition>> GetVehiclePositionListAsyncByDateRange(Guid vehicleId, DateTime fromDatem, DateTime toDate);
    }
}
