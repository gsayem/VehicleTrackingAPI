using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace VehicleTracking.Interfaces.Repository {
    public interface IDataContext {
        IEnumerable<EntityEntry> GetChangeTrackerEntries();
        DbSet<TEntity> Set<TEntity>() where TEntity : class;
        EntityEntry<TEntity> Entry<TEntity>(TEntity entity) where TEntity : class;
        EntityEntry Entry(object entity);
        int SaveChanges();
        Task<int> SaveChangesAsync();

    }
}
