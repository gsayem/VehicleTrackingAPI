using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using VehicleTracking.IdentityServer.Model;

namespace VehicleTracking.Data.Mappings.Identity.Mappings
{
    public class PersistedGrantContextMappings : IBaseModelMapper
    {
        public void MapEntity(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<PersistedGrant>(grant =>
            {
                grant.ToTable("PersistedGrants");

                grant.Property(x => x.Key).HasMaxLength(200);
                grant.Property(x => x.Type).HasMaxLength(50);
                grant.Property(x => x.SubjectId).HasMaxLength(200);
                grant.Property(x => x.ClientId).HasMaxLength(200).IsRequired();
                grant.Property(x => x.CreationTime).IsRequired();
                grant.Property(x => x.Expiration).IsRequired();
                grant.Property(x => x.Data).IsRequired();

                grant.HasKey(x => new { x.Key, x.Type });

                grant.HasIndex(x => x.SubjectId);
                grant.HasIndex(x => new { x.SubjectId, x.ClientId });
                grant.HasIndex(x => new { x.SubjectId, x.ClientId, x.Type });
            });
        }
    }
}
