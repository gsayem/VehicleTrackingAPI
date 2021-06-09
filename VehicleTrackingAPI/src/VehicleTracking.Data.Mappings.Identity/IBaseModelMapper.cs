using Microsoft.EntityFrameworkCore;
using System;

namespace VehicleTracking.Data.Mappings.Identity {
    interface IBaseModelMapper {
        void MapEntity(ModelBuilder modelBuilder);
    }
}
