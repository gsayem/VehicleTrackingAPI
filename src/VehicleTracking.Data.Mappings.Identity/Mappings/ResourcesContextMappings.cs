using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using VehicleTracking.IdentityServer.Model;

namespace VehicleTracking.Data.Mappings.Identity.Mappings
{
    public class ResourcesContextMappings : IBaseModelMapper
    {
        public void MapEntity(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<IdentityResource>(identityResource =>
            {
                identityResource.ToTable("IdentityResources").HasKey(x => x.Id);

                identityResource.Property(x => x.Name).HasMaxLength(200).IsRequired();
                identityResource.Property(x => x.DisplayName).HasMaxLength(200);
                identityResource.Property(x => x.Description).HasMaxLength(1000);

                identityResource.HasIndex(x => x.Name).IsUnique();

                identityResource.HasMany(x => x.UserClaims).WithOne(x => x.IdentityResource).IsRequired().OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<IdentityClaim>(claim =>
            {
                claim.ToTable("IdentityClaims").HasKey(x => x.Id);

                claim.Property(x => x.Type).HasMaxLength(200).IsRequired();
            });


            modelBuilder.Entity<ApiResource>(apiResource =>
            {
                apiResource.ToTable("ApiResources").HasKey(x => x.Id);

                apiResource.Property(x => x.Name).HasMaxLength(200).IsRequired();
                apiResource.Property(x => x.DisplayName).HasMaxLength(200);
                apiResource.Property(x => x.Description).HasMaxLength(1000);

                apiResource.HasIndex(x => x.Name).IsUnique();

                apiResource.HasMany(x => x.Secrets).WithOne(x => x.ApiResource).IsRequired().OnDelete(DeleteBehavior.Cascade);
                apiResource.HasMany(x => x.Scopes).WithOne(x => x.ApiResource).IsRequired().OnDelete(DeleteBehavior.Cascade);
                apiResource.HasMany(x => x.UserClaims).WithOne(x => x.ApiResource).IsRequired().OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<ApiSecret>(apiSecret =>
            {
                apiSecret.ToTable("ApiSecrets").HasKey(x => x.Id);

                apiSecret.Property(x => x.Description).HasMaxLength(1000);
                apiSecret.Property(x => x.Value).HasMaxLength(2000);
                apiSecret.Property(x => x.Type).HasMaxLength(250);
            });

            modelBuilder.Entity<ApiResourceClaim>(apiClaim =>
            {
                //apiClaim.ToTable("ApiClaims").HasKey(x => x.Id);
                apiClaim.ToTable("ApiResourceClaims").HasKey(x => x.Id);
                apiClaim.Property(x => x.Type).HasMaxLength(200).IsRequired();
            });

            modelBuilder.Entity<ApiScope>(apiScope =>
            {
                apiScope.ToTable("ApiScopes").HasKey(x => x.Id);

                apiScope.Property(x => x.Name).HasMaxLength(200).IsRequired();
                apiScope.Property(x => x.DisplayName).HasMaxLength(200);
                apiScope.Property(x => x.Description).HasMaxLength(1000);

                apiScope.HasIndex(x => x.Name).IsUnique();

                apiScope.HasMany(x => x.UserClaims).WithOne(x => x.ApiScope).IsRequired().OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<ApiScopeClaim>(apiScopeClaim =>
            {
                apiScopeClaim.ToTable("ApiScopeClaims").HasKey(x => x.Id);

                apiScopeClaim.Property(x => x.Type).HasMaxLength(200).IsRequired();
            });
            modelBuilder.Entity<PasswordHistory>(passwordHistory =>
            {
                passwordHistory.ToTable("PasswordHistories");
                passwordHistory.HasKey(x => x.Id).IsClustered(true);
                passwordHistory.Property(x => x.PasswordHash).IsRequired();
                passwordHistory.Property(x => x.CreatedDate).IsRequired();
            });
        }
    }
}
