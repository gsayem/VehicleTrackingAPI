using VehicleTracking.Interfaces.Repository;
using VehicleTracking.Model;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace VehicleTracking.Repository {
    public class Repository<TEntity> : UOWRepository<TEntity> where TEntity : class, IBaseModel {
        public Repository(IDataContext dataContext) : base(dataContext) {
        }

        public override void Add(TEntity entity) {
            base.Add(entity);
            DataContext.SaveChanges();
        }

        public override void Update(TEntity entity) {
            base.Update(entity);
            DataContext.SaveChanges();
        }

        public override void Remove(TEntity entity) {
            base.Remove(entity);
            DataContext.SaveChanges();
        }

        public override async Task<Guid> AddAsync(TEntity entity) {
            base.Add(entity);
            await DataContext.SaveChangesAsync();
            return entity.Id;
        }

        public override async Task<int> UpdateAsync(TEntity entity) {
            base.Update(entity);
            return await DataContext.SaveChangesAsync();
        }

        public override async Task<int> RemoveAsync(TEntity entity) {
            base.Remove(entity);
            return await DataContext.SaveChangesAsync();
        }

        public override async Task<IList<TEntity>> GetAllAsync() {
            return await DataContext.Set<TEntity>().ToListAsync();
        }

        public override async Task<int> CountAsync() {
            return await DataContext.Set<TEntity>().CountAsync();
        }

        public override async Task<IList<TEntity>> FindAllAsync(Expression<Func<TEntity, bool>> predicate) {
            return await DataContext.Set<TEntity>().Where(predicate).ToListAsync();
        }

        public override async Task<TEntity> FindAsync(Expression<Func<TEntity, bool>> predicate) {
            return await DataContext.Set<TEntity>().SingleOrDefaultAsync(predicate);
        }

        public override async Task<TEntity> GetByIdAsync(Guid id) {
            return await DataContext.Set<TEntity>().SingleOrDefaultAsync(i => i.Id == id);
        }
    }
}
