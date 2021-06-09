using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace VehicleTracking.IdentityServer.Web.API.Helpers {
    public interface IUtilHelper {
        string BaseUrl(HttpRequest request);
        Task<string> ParseTemplateAsync(string templateFileName, object model, dynamic expandoObject);
    }
}
