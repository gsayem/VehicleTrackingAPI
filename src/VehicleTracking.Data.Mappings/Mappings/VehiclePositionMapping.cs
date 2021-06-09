using VehicleTracking.Model;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace VehicleTracking.Data.Mappings.Mappings
{
    public class VehiclePositionMapping : IBaseModelMapper
    {
        public void MapEntity(ModelBuilder modelBuilder)
        {

            modelBuilder.Entity<VehiclePosition>().ToTable("VehiclePositions");
            #region Base Class Items
            modelBuilder.Entity<VehiclePosition>().HasKey(s => s.Id).IsClustered(true);
            modelBuilder.Entity<VehiclePosition>().Property(g => g.CreatedDate).IsRequired();
            modelBuilder.Entity<VehiclePosition>().Property(g => g.UpdatedDate).IsRequired();
            #endregion

            
            //modelBuilder.Entity<VehiclePosition>().Property(g => g.Latitude).HasPrecision(18, 4);
            //modelBuilder.Entity<VehiclePosition>().Property(g => g.Longitude).HasPrecision(18, 4);

        }
    }
}
