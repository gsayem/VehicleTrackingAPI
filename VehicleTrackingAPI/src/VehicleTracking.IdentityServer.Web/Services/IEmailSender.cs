using System.Threading.Tasks;

namespace VehicleTracking.IdentityServer.Web.API.Services {
    public interface IEmailSender {
        Task SendEmailAsync(string email, string subject, string message);
    }
}
