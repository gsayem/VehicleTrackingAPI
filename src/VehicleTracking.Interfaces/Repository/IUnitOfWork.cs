using System.Threading.Tasks;
using VehicleTracking.Model;

namespace VehicleTracking.Interfaces.Repository {
    public interface IUnitOfWork {
        void SaveChanges();
        Task SaveChangesAsync();
        IRepository<TSet> GetRepository<TSet>() where TSet : class, IBaseModel;
        IRepositoryAsync<TSet> GetRepositoryAsync<TSet>() where TSet : class, IBaseModel;
    }
}
