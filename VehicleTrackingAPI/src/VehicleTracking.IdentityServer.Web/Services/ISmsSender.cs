using System.Threading.Tasks;

namespace VehicleTracking.IdentityServer.Web.API.Services {
    public interface ISmsSender {
        Task SendSmsAsync(string number, string message);
    }
}
