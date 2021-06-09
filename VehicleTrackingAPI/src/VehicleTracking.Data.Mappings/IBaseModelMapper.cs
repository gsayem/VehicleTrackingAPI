using Microsoft.EntityFrameworkCore;

namespace VehicleTracking.Data.Mappings {
    interface IBaseModelMapper
    {
        void MapEntity(ModelBuilder modelBuilder);
    }
}
