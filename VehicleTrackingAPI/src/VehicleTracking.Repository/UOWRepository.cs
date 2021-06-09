using VehicleTracking.Interfaces.Repository;
using VehicleTracking.Model;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace VehicleTracking.Repository {
    public class UOWRepository<TEntity> : IRepository<TEntity>, IRepositoryAsync<TEntity> where TEntity : class, IBaseModel {
        public UOWRepository(IDataContext dataContext) {
            if (dataContext == null) throw new ArgumentNullException(nameof(dataContext));

            this.DataContext = dataContext;
        }

        public IDataContext DataContext { get; }

        public virtual void Add(TEntity entity) {
            DataContext.Set<TEntity>().Add(entity);
        }

        public virtual void Remove(TEntity entity) {
            DataContext.Entry(entity).State = EntityState.Deleted;
        }

        public virtual TEntity GetById(Guid id) {
            return DataContext.Set<TEntity>().SingleOrDefault(i => i.Id == id);
        }

        public virtual IList<TEntity> GetAll() {
            return DataContext.Set<TEntity>().ToList();
        }

        public virtual void Update(TEntity entity) {
            DataContext.Entry(entity).State = EntityState.Modified;
        }

        public virtual int Count() {
            return DataContext.Set<TEntity>().Count();
        }

        public virtual TEntity Find(Expression<Func<TEntity, bool>> predicate) {
            return DataContext.Set<TEntity>().SingleOrDefault(predicate);
        }

        public virtual IList<TEntity> FindAll(Expression<Func<TEntity, bool>> predicate) {
            return DataContext.Set<TEntity>().Where(predicate).ToList();
        }

        public virtual void SaveChanges() {
            DataContext.SaveChanges();
        }

        public virtual async Task AddAsync(TEntity entity) {
            await DataContext.Set<TEntity>().AddAsync(entity);
        }

        public virtual async Task RemoveAsync(TEntity entity) {
            DataContext.Entry(entity).State = EntityState.Deleted;
        }

        public virtual async Task<TEntity> GetByIdAsync(Guid id) {
            return await DataContext.Set<TEntity>().SingleOrDefaultAsync(i => i.Id == id);
        }

        public virtual async Task<IList<TEntity>> GetAllAsync() {
            return await DataContext.Set<TEntity>().ToListAsync();
        }

        public virtual async Task UpdateAsync(TEntity entity) {
            DataContext.Entry(entity).State = EntityState.Modified;
        }

        public virtual async Task<int> CountAsync() {
            return await DataContext.Set<TEntity>().CountAsync();
        }

        public virtual async Task<TEntity> FindAsync(Expression<Func<TEntity, bool>> predicate) {
            return await DataContext.Set<TEntity>().SingleOrDefaultAsync(predicate);
        }

        public virtual async Task<IList<TEntity>> FindAllAsync(Expression<Func<TEntity, bool>> predicate) {
            return await DataContext.Set<TEntity>().Where(predicate).ToListAsync();
        }
    }
}
