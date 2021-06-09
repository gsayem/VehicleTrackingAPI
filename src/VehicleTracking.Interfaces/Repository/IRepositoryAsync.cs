using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using VehicleTracking.Model;

namespace VehicleTracking.Interfaces.Repository {
    public interface IRepositoryAsync<TEntity> where TEntity : class, IBaseModel {
        Task AddAsync(TEntity entity);
        Task RemoveAsync(TEntity entity);
        Task<TEntity> GetByIdAsync(Guid id);
        Task<IList<TEntity>> GetAllAsync();
        Task UpdateAsync(TEntity entity);
        Task<int> CountAsync();
        Task<TEntity> FindAsync(Expression<Func<TEntity, bool>> predicate);
        Task<IList<TEntity>> FindAllAsync(Expression<Func<TEntity, bool>> predicate);
    }
}
