using VehicleTracking.Model;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace VehicleTracking.Data.Mappings.Mappings
{
    public class VehicleMapping : IBaseModelMapper
    {
        public void MapEntity(ModelBuilder modelBuilder)
        {

            modelBuilder.Entity<Vehicle>().ToTable("Vehicles");
            #region Base Class Items
            modelBuilder.Entity<Vehicle>().HasKey(s => s.Id).IsClustered(true);
            modelBuilder.Entity<Vehicle>().Property(g => g.CreatedDate).IsRequired();
            modelBuilder.Entity<Vehicle>().Property(g => g.UpdatedDate).IsRequired();
            #endregion

            modelBuilder.Entity<Vehicle>().Property(g => g.Name);
            modelBuilder.Entity<Vehicle>().Property(g => g.RegistrationNumber);
            modelBuilder.Entity<Vehicle>().Property(g => g.AspNetUserId);
            

            modelBuilder.Entity<Vehicle>().HasMany(c => c.VehiclePositions).WithOne(b => b.Vehicle).OnDelete(DeleteBehavior.Restrict).IsRequired();
        }
    }
}
