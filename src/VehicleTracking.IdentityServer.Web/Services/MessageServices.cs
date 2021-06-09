using Microsoft.Extensions.Options;
using System.Threading.Tasks;
using VehicleTracking.IdentityServer.Services;
using VehicleTracking.Web.Common;

namespace VehicleTracking.IdentityServer.Web.API.Services {
    public class AuthMessageSender : IEmailSender, ISmsSender {
        EmailSettings _mailSettings;
        IClientService _clientService;
        public AuthMessageSender(IOptions<EmailSettings> mailSettings, IClientService clientService) {
            _mailSettings = mailSettings.Value;
            _clientService = clientService;
        }

        public async Task SendEmailAsync(string email, string subject, string message) {
            // SendEmail Funtionalities
            await Task.FromResult(0);
        }

        public async Task SendSmsAsync(string number, string message) {
            // Plug in your SMS service here to send a text message.
            await Task.FromResult(0);
        }
    }
}
