using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using VehicleTracking.Model;

namespace VehicleTracking.Interfaces.Repository {
    public interface IRepository<TEntity> where TEntity : class, IBaseModel {
        void Add(TEntity entity);
        void Remove(TEntity entity);
        IList<TEntity> GetAll();
        TEntity GetById(Guid id);
        void Update(TEntity entity);
        int Count();
        TEntity Find(Expression<Func<TEntity, bool>> predicate);
        IList<TEntity> FindAll(Expression<Func<TEntity, bool>> predicate);
    }
}
